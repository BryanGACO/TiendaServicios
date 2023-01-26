using System.Text.Json;
using TiendaServicios.Api.CarritoCompra.RemoteModel;
using TiendaServicios.Api.CarritoCompra.Services.Interfaces;

namespace TiendaServicios.Api.CarritoCompra.Services
{
    public class LibroService : ILibroService
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<LibroService> _logger;
        public LibroService(IHttpClientFactory httpClient, ILogger<LibroService> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); ;
        }

        public async Task<(bool IsSuccess, LibroRemote Libro, string ErrorMessage)> GetLibro(Guid libroId)
        {
            try
            {
                var client = _httpClient.CreateClient("Libros");
                var response = await client.GetAsync($"api/LibroMaterial/{libroId}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var resultado = JsonSerializer.Deserialize<LibroRemote>(content, options);
                    return (true, resultado, null);
                }

                return (response.IsSuccessStatusCode, null, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
