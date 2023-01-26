using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Persistencia;

namespace TiendaServicios.Api.Libro.Aplicacion
{
    public class ConsultaFiltro
    {
        public class LibroMaterialUnico : IRequest<LibroMaterialDto>
        {
            public Guid? LibroMaterialGuid { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<LibroMaterialUnico> {
            public EjecutaValidacion()
            {
                RuleFor(x=> x.LibroMaterialGuid).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<LibroMaterialUnico, LibroMaterialDto>
        {
            private readonly ContextoLibreria _contexto;
            private readonly IMapper _mapper;

            public Manejador(ContextoLibreria contexto, IMapper mapper)
            {
                _contexto = contexto;
                _mapper = mapper;
            }

            public async Task<LibroMaterialDto> Handle(LibroMaterialUnico request, CancellationToken cancellationToken)
            {
                var libroMaterial = await _contexto.LibreriaMaterial.Where(l => l.LibreriaMaterialId == request.LibroMaterialGuid).FirstOrDefaultAsync(cancellationToken);

                if (libroMaterial == null) throw new Exception("No se encontró el libro solicitado");

                var libroMaterialDto = _mapper.Map<LibreriaMaterial, LibroMaterialDto>(libroMaterial);

                return libroMaterialDto;
            }
        }
    }
}
