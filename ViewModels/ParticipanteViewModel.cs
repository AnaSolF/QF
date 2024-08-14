//Borramos referencias y usamos
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using QF.DataAccess;
using QF.DTOs;
using QF.Utilidades;
using QF.Modelos;

namespace QF.ViewModels
{
    //Clase pública y partial. Hereda ObservableObject , IQueryAttributable (Implementar interfaz)
    public partial class ParticipanteViewModel : ObservableObject, IQueryAttributable
    {
        //Creamos las propiedades que vamos a utilizar para nuestro formulario
        //Variable readonly que representa base de datos
        private readonly ProjectDBContext? _dbContext;

        [ObservableProperty]
        private ParticipanteDTO participanteDto = new();

        [ObservableProperty]
        private string tituloPagina;

        private int IdParticipante;

        [ObservableProperty]
        private bool loadingEsVisible = false;

        public ParticipanteViewModel(ProjectDBContext context)
        {
            _dbContext = context;
            ParticipanteDto.FechaInscripcion = DateTime.Now;
        }


        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var id =int.Parse(query["id"].ToString());
            //Lo convertimos en entero
            IdParticipante = id;

            if(IdParticipante == 0){
                TituloPagina = "Nuevo Participante";
            }
            else
            {
                TituloPagina = "Editar Participante";
                LoadingEsVisible = true;

                //Creamos una tarea asìncrona en un hilo diferente.
                //Consulta asìncrona a la base de datos, para poder encontrar la info del participante
                //(Accedemos al _dbContext.Participantes (que es la tabla participantes)
                //y filtramos o cotejamos segùn el IdParticipante
                //y le asignamos todas las propiedades de nuestro participante encontrado
                await Task.Run(async () =>
                {
                    var encontrado = await _dbContext.Participantes.FirstAsync(e => e.IdParticipante == IdParticipante);
                   ParticipanteDto.IdParticipante = encontrado.IdParticipante;
                   ParticipanteDto.Nombre= encontrado.Nombre;
                   ParticipanteDto.Correo = encontrado.Correo;
                   ParticipanteDto.FechaInscripcion = encontrado.FechaInscripcion;
                    //Regresamos al hilo principal para que ejecute una acciòn: Que loading ya no se visualice
                    MainThread.BeginInvokeOnMainThread(() => { LoadingEsVisible = false; });
                });
            }

        }
        //Mètodo para ejecutar acciòn guardar ( privado, asìncrono, ejecuta una Task-tarea-) 
        //Creo que se le asigna al botòn, dos lògicas. Un a que permite crear y otra que permite editar las propiedades
        [RelayCommand]
        private async Task Guardar()
        {
            LoadingEsVisible= true;
            //Instancia de mensaje. Tuve que corregir un error respecto de la creaciòn del objeto (VER)
            var mensaje = new ParticipanteMensaje
            {
                ParticipanteDto = new ParticipanteDTO()
            };
            //Otra tarea asìncrona, en caso de que IdParticipante ==0

            await Task.Run(async () =>
            {
            if(IdParticipante==0){
                    //Estamos creando
                    var tbParticipante = new Participante
                    {
                        Nombre = ParticipanteDto.Nombre,
                        Correo = ParticipanteDto.Correo,
                        FechaInscripcion = ParticipanteDto.FechaInscripcion,
                    };

                    //Agrego el nuevo participànte a la base de datos y guardo
                    _dbContext.Participantes.Add(tbParticipante);
                    await _dbContext.SaveChangesAsync();

                    ParticipanteDto.IdParticipante= tbParticipante.IdParticipante;
                    mensaje = new ParticipanteMensaje()
                    {
                        //Este participante debe ser creado?
                        EsCrear = true,
                        ParticipanteDto = ParticipanteDto

                    };
                }
                else
                {    //Ediciòn. Abre bd. Este partricipante ya fue creado y debe ser encontrado
                    var encontrado = await _dbContext.Participantes.FirstAsync(e=>e.IdParticipante==IdParticipante);
                    encontrado.Nombre = ParticipanteDto.Nombre;
                    encontrado.Correo = ParticipanteDto.Correo;
                    encontrado.FechaInscripcion= ParticipanteDto.FechaInscripcion;
                    //Guardamos
                    await _dbContext.SaveChangesAsync();

                    mensaje = new ParticipanteMensaje()
                    {
                        //No hemos creado un participante
                        EsCrear = false,
                        ParticipanteDto = ParticipanteDto
                    };
                }
            //Volvemos al hilo principal (Lo anterior corrìa en un hilo separado)
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    LoadingEsVisible = false;
                    WeakReferenceMessenger.Default.Send(new ParticipanteMensajeria(mensaje));
                    //Pasar a la siguiente pantalla?
                    await Shell.Current.Navigation.PopAsync();
                });

            });
                   
        }

    }
}
