using MediatR;
using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.CarritoCompra.Aplicacion;
using TiendaServicios.Api.CarritoCompra.Persistencia;
using TiendaServicios.Api.CarritoCompra.Services;
using TiendaServicios.Api.CarritoCompra.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<CarritoContexto>(options => {
    options.UseMySql(builder.Configuration.GetConnectionString("ConexionDb"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("ConexionDb")));
});

builder.Services.AddMediatR(typeof(Nuevo.Ejecuta).Assembly);

builder.Services.AddHttpClient("Libros", config => {
    config.BaseAddress = new Uri(builder.Configuration["Services:Libros"]);
});

builder.Services.AddScoped<ILibroService, LibroService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
