using MediatR;
using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.CarritoCompra.Persistencia;
using TiendaServicios.Api.CarritoCompra.RemoteModel;
using TiendaServicios.Api.CarritoCompra.Services.Interfaces;

namespace TiendaServicios.Api.CarritoCompra.Aplicacion
{
    public class Consulta
    {
        public class Ejecuta : IRequest<CarritoDto>
        {
            public int CarritoSesionId { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta, CarritoDto>
        {
            private readonly CarritoContexto _contexto;
            private readonly ILibroService _libroService;
            public Manejador(CarritoContexto contexto, ILibroService libroService)
            {
                _contexto = contexto;
                _libroService = libroService;
            }

            public async Task<CarritoDto> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var carritoSesion = await _contexto.CarritoSesion.Include(c=> c.ListaDetalle).FirstOrDefaultAsync(x => x.CarritoSesionId == request.CarritoSesionId, cancellationToken);

                var detallesCarritoSesion = carritoSesion.ListaDetalle;

                var listaCarritoDto = new List<CarritoDetalleDto>();

                foreach (var carritoSesionDetalle in detallesCarritoSesion)
                {
                    var response = await _libroService.GetLibro(Guid.Parse(carritoSesionDetalle.ProductoSeleccionado));
                    if (response.IsSuccess)
                    {
                        LibroRemote libro = response.Libro;

                        var carritoDetalle = new CarritoDetalleDto
                        {
                            TituloLibro = libro.Titulo,
                            FechaPublicacion = libro.FechaPublicacion,
                            LibroId = libro.LibreriaMaterialId                            
                        };
                        listaCarritoDto.Add(carritoDetalle);
                    }
                }

                var carritoDto = new CarritoDto
                {
                    CarritoId = carritoSesion.CarritoSesionId,
                    FechaCreacionSesion = carritoSesion.FechaCreacion,
                    ListaProducto = listaCarritoDto
                };

                return carritoDto;
                
            }
        }

    }
}
