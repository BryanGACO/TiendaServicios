using MediatR;
using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.Autor.Modelo;
using TiendaServicios.Api.Autor.Persistencia;
using AutoMapper;

namespace TiendaServicios.Api.Autor.Aplicacion
{
    public class Consulta
    {
        // Se crean 2 classes 
        // La primera clase que se encargue de recibir los parametros, hereda de la interfaz IRequest<TipoDeDatoADevolver>
        // La segunda es la encargada de realizar la consulta

        public class ListaAutor : IRequest<List<AutorDto>>
        {
        }

        public class Manejador : IRequestHandler<ListaAutor, List<AutorDto>>
        {
            private readonly ContextoAutor _contexto;
            private readonly IMapper _mapper;
            public Manejador(ContextoAutor contexto, IMapper mapper)
            {
                _contexto = contexto;
                _mapper = mapper;
            }

            public async Task<List<AutorDto>> Handle(ListaAutor request, CancellationToken cancellationToken)
            {
                var autorLibros = await _contexto.AutorLibro.ToListAsync(cancellationToken);

                var autoresDto = _mapper.Map<List<AutorLibro>, List<AutorDto>>(autorLibros);
                return autoresDto;
            }
        }
    }
}
