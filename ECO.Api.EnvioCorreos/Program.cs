using ECO.Api.EnvioCorreos.Middlewares;
using ECO.Aplicacion.CasosUso.Implementaciones;
using ECO.Aplicacion.CasosUso.Interfaces;
using ECO.Aplicacion.Servicios.Implementaciones;
using ECO.Aplicacion.Servicios.Interfaces;
using log4net;
using log4net.Config;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICorreoServicio, CorreoServicio>();
builder.Services.AddScoped<IApiResponse, ApiResponse>();

// Configuración de log4net
var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
builder.Services.AddLogging(loggingBuilder => {loggingBuilder.AddLog4Net();});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<MiddlewareExcepcionesGlobales>();

app.UseAuthorization();

app.MapControllers();

app.Run();
