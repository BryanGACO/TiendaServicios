using FluentValidation;
using MediatR;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Persistencia;

namespace TiendaServicios.Api.Libro.Aplicacion
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {            
            public string Titulo { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public Guid? AutorLibro { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(e => e.Titulo).NotEmpty();
                RuleFor(e => e.FechaPublicacion).NotNull();
                RuleFor(e => e.AutorLibro).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly ContextoLibreria _contexto;

            public Manejador(ContextoLibreria contexto)
            {
                _contexto = contexto;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var libreriaMaterial = new LibreriaMaterial
                {
                    AutorLibro = request.AutorLibro,
                    FechaPublicacion= request.FechaPublicacion,
                    Titulo = request.Titulo,
                    LibreriaMaterialId = Guid.NewGuid()
                };

                _contexto.LibreriaMaterial.Add(libreriaMaterial);
                var value = await _contexto.SaveChangesAsync(cancellationToken);

                if (value > 0)
                    return Unit.Value;

                throw new Exception("No se pudo guardar el libro");
                throw new NotImplementedException();
            }
        }
    }
}
