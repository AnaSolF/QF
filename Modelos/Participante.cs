//Agregamos
using System.ComponentModel.DataAnnotations;

namespace QF.Modelos
{   //Cambiamos a public
    public class Participante
    {
        [Key] //Clave primaria
        public int IdParticipante { get; set; }
        public required string Nombre { get; set; }
        public required string Correo { get; set; }
        public DateTime FechaInscripcion { get; set; }
    }
}
