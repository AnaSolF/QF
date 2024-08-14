using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using QF.DataAccess;
using QF.DTOs;
using QF.Utilidades;
using QF.Views;
using System.Collections.ObjectModel;


namespace QF.ViewModels
{
    public partial class CreatePartViewModel : ObservableObject
    {
        //Variable readonly que representa base de datos
        private readonly ProjectDBContext _dbContext;

        //Lista de empleados
        [ObservableProperty]
        private ObservableCollection<ParticipanteDTO> listaParticipante = new ObservableCollection<ParticipanteDTO>();

        //Constructor (con ctor + tab)
        public CreatePartViewModel(ProjectDBContext context)
        {
            _dbContext = context;
            MainThread.BeginInvokeOnMainThread(new Action(async () => await Obtener()));

            //Suscripción a ParticipanteMensajeria para poder extraer la otra informaciòn.
            //Recibe dos paràmetros, receptor y mensaje(r,m). Sòlo usamos mensaje.
            WeakReferenceMessenger.Default.Register<ParticipanteMensajeria>(this, (r, m) =>
            {
                ParticipanteMensajeRecibido(m.Value);//Obtenemos el valor de m
            });


        }

        //Mètodo Obtener() para obtener desde la bd toda la lista de participantes.
        //Lo ejecutamos en el constructor, mediante una acción asìncrona (new Action)
        public async Task Obtener()
        {
            var lista = await _dbContext.Participantes.ToListAsync();
            //Validamos si la lista (tabla) tiene elementos o no
            if (lista.Any())
            {
                //Iteramos cada elemento de nuestra lista
                foreach (var participante in lista)
                {
                    ListaParticipante.Add(new ParticipanteDTO
                    {
                        IdParticipante = participante.IdParticipante,
                        Nombre = participante.Nombre,
                        Correo = participante.Correo,
                        FechaInscripcion = participante.FechaInscripcion,
                    });
                }
            }
        }

        //Mètodo para que desde el formulario Cuàles son los datos que recibe.
        //En este mètodo usamos otro que està dentro del constructor para obtener el valor del mensaje
        private void ParticipanteMensajeRecibido(ParticipanteMensaje participanteMensaje)
        {
            //Obtenemos el DTO que està dentro del mensaje. Si participanteMensaje es para crear 
            var participanteDto = participanteMensaje.ParticipanteDto;
            if (participanteMensaje.EsCrear)
            {
                ListaParticipante.Add(participanteDto);
            }
            else //El participante es para editar
            {
                var encontrado = ListaParticipante.First(e => e.IdParticipante == participanteDto.IdParticipante);
                encontrado.Nombre = participanteDto.Nombre;
                encontrado.Correo = participanteDto.Correo;
                encontrado.FechaInscripcion = participanteDto.FechaInscripcion;
            }
        }

        //Tarea para crear una uri hacia ParticipantePage(Views)
        //y còmo paràmetro del id le està pasando 0, por lo tanto lo tiene que crear
        [RelayCommand]
        private async Task Crear()
        {
            var uri = $"{nameof(ParticipantePage)}?id=0";
            //Dirigimos hacia esa pàgina
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        //Recibimos ParticipanteDTO por lo tanto, al ser el id distinto de 0, Edita
        //redirecciona hacia la pàgina ParticipantePage
        private async Task Editar(ParticipanteDTO participanteDto)
        {
            var uri = $"{nameof(ParticipantePage)}?id={participanteDto.IdParticipante}";
            //Redirige hacia ParticipantePage(que es la uri)
            await Shell.Current.GoToAsync(uri);
        }

        //Eliminar participante
        [RelayCommand]
        private async Task Eliminar(ParticipanteDTO participanteDto)
        {
            bool answer = await Shell.Current.DisplayAlert("Mensaje", "Desea eliminar el empleado?", "Si", "No");

            if (answer)
            {
                var encontrado = await _dbContext.Participantes.FirstAsync(e => e.IdParticipante == participanteDto.IdParticipante);

                _dbContext.Participantes.Remove(encontrado);
                await _dbContext.SaveChangesAsync();
                ListaParticipante.Remove(participanteDto);
                //No redirige hacia ninguna pàgina

            }

        }
    }
}
