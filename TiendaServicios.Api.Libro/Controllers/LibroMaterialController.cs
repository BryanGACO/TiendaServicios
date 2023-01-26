using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TiendaServicios.Api.Libro.Aplicacion;

namespace TiendaServicios.Api.Libro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibroMaterialController : ControllerBase
    {
        private IMediator _mediator;
        public LibroMaterialController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LibroMaterialDto>> GetLibro([FromRoute, Required] string id)
        {
            var libros = await _mediator.Send(new ConsultaFiltro.LibroMaterialUnico { LibroMaterialGuid = Guid.Parse(id) });
            return libros;
        }
        [HttpGet]
        public async Task<ActionResult<List<LibroMaterialDto>>> GetLibros()
        {
            var libros = await _mediator.Send(new Consulta.Ejecuta());
            return libros;
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Nuevo.Ejecuta request)
        {
            return await _mediator.Send(request);
        }
    }
}

