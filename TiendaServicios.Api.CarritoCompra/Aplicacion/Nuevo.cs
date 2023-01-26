using MediatR;
using TiendaServicios.Api.CarritoCompra.Modelo;
using TiendaServicios.Api.CarritoCompra.Persistencia;

namespace TiendaServicios.Api.CarritoCompra.Aplicacion
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public List<string> ProductoLista { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CarritoContexto _contexto;
            public Manejador(CarritoContexto contexto)
            {
                _contexto = contexto;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var carritoSesion = new CarritoSesion { FechaCreacion = DateTime.Now };

                _contexto.CarritoSesion.Add(carritoSesion);
                var value = await _contexto.SaveChangesAsync(cancellationToken);

                if (value == 0)
                    throw new Exception("Se ha presentado un error al insertar el carrito sesión");

                foreach (string producto in request.ProductoLista)
                {
                    var detalleSesion = new CarritoSesionDetalle { CarritoSesionId = carritoSesion.CarritoSesionId, FechaCreacion = DateTime.Now, ProductoSeleccionado = producto };

                    _contexto.CarritoSesionDetalle.Add(detalleSesion);
                }

                value = await _contexto.SaveChangesAsync(cancellationToken);

                if (value > 0)
                    return Unit.Value;

                throw new Exception("No se pudo insertar el detalle del carrito de compras");
            }
        }
    }
}
