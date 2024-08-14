using CommunityToolkit.Mvvm.ComponentModel;

namespace QF.DTOs
{ //Configuramos clase para que sea un objeto
    //Copiamos propiedades del modelo Participante
  //Agregamos notación [ObservableProperty]
  //Quitamos get/set
  //Colocamos primer letra en minúsculas y ;
    public partial class ParticipanteDTO : ObservableObject
    {
        [ObservableProperty]
        public int idParticipante;

        [ObservableProperty]
        public  string? nombre;

        [ObservableProperty]
        public string? correo;

        [ObservableProperty]
        public DateTime fechaInscripcion;
    }
}
