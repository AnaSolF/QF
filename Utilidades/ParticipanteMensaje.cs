using QF.DTOs;

namespace QF.Utilidades
{
    //Clase public
    public class ParticipanteMensaje
    {
        //Dentro un boolean, para saber si el participante esta creado no debe crearse. Es false por defecto 
        public bool EsCrear {get;set;}

        //Nueva propiedad ParticipanteDTO para devolver el participante creado o editado
        public required ParticipanteDTO ParticipanteDto { get;set;}
    }
}
