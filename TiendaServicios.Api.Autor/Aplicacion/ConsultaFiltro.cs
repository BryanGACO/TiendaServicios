using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.Autor.Modelo;
using TiendaServicios.Api.Autor.Persistencia;

namespace TiendaServicios.Api.Autor.Aplicacion
{
    public class ConsultaFiltro
    {
        public class AutorUnico : IRequest<AutorDto>
        {
            public string AutorGuid { get; set; }
        }

        public class Manejador : IRequestHandler<AutorUnico, AutorDto>
        {
            public readonly ContextoAutor _contexto;
            public readonly IMapper _mapper;
            public Manejador(ContextoAutor contexto, IMapper mapper)
            {
                _contexto = contexto;
                _mapper = mapper;
            }

            public async Task<AutorDto> Handle(AutorUnico request, CancellationToken cancellationToken)
            {
                var autorLibro = await _contexto.AutorLibro.Where(a => a.AutorLibroGuid == request.AutorGuid).FirstOrDefaultAsync(cancellationToken);
                if (autorLibro == null) { throw new Exception("No se encontró el autor"); }

                var autorLibroDto = _mapper.Map<AutorLibro, AutorDto>(autorLibro);
                return autorLibroDto;
            }
        }
    }
}
