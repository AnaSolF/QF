//Agregamos
using QF.Modelos;
using QF.Utilidades;

using Microsoft.EntityFrameworkCore;
namespace QF.DataAccess
{ //Hacemos public y hacemos que herede de DBContext
    public class ProjectDBContext : DbContext
    {
        //Tablas de la base de datos(Nombre del modelo(<Empleado>) y nombre de la tabla (Empleados)
        public DbSet<Participante>Participantes { get; set; }

        //Sobreescribimos método OnConfiguring para crear la cadena de conexión
        //usamos el método que habíamos creado y le pasamos cómo parámetro el nombre de la base de datos
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Creamos la conexión (Observar comillas)
            string conexionDB = $"Filename = {ConexionDB.DevolverRuta("proyectoFree.db")}";  
            optionsBuilder.UseSqlite(conexionDB);
        }

        //Creamos la tabla(Entity Participante, primary key IdParticipante
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Participante>(entity =>
            {
                //Clave primaria
                entity.HasKey(col => col.IdParticipante);
                //Es requerido y que cada vez que generamos un participante se genere automáticamente
                entity.Property(col=> col.IdParticipante).IsRequired().ValueGeneratedOnAdd();
            });  
        }

        //Nos vamos al archivo MauiProgram y continuamos debajo de builder 

    }   
}
