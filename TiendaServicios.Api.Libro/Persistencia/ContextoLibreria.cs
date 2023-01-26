using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.Libro.Modelo;

namespace TiendaServicios.Api.Libro.Persistencia
{
    public class ContextoLibreria: DbContext
    {
        // Se agrega por un bug que tiene la herramienta de testing.
        public ContextoLibreria() {}

        //Esto se hace para poder setear la cadena de conexión desde los archivos de configuración.
        public ContextoLibreria(DbContextOptions<ContextoLibreria> options):base(options) { }
        public virtual DbSet<LibreriaMaterial> LibreriaMaterial { get; set; }
    }
}
