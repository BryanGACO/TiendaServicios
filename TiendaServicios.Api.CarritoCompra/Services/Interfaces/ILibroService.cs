using TiendaServicios.Api.CarritoCompra.RemoteModel;

namespace TiendaServicios.Api.CarritoCompra.Services.Interfaces
{
    public interface ILibroService
    {
        Task<(bool IsSuccess, LibroRemote Libro, string ErrorMessage)> GetLibro(Guid libroId);
    }
}
