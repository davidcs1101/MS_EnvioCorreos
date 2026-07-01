using ECO.Api.EnvioCorreos.Middlewares;
using ECO.Aplicacion.CasosUso.Implementaciones;
using ECO.Aplicacion.CasosUso.Interfaces;
using ECO.Aplicacion.Servicios.Implementaciones;
using ECO.Aplicacion.Servicios.Interfaces;
using ECO.Aplicacion.ServiciosExternos;
using ECO.Aplicacion.ServiciosExternos.config;
using ECO.Aplicacion.ServiciosExternos.Mapeo;
using ECO.DataAccess;
using ECO.Dominio.Repositorio;
using ECO.Dominio.Repositorio.UnidadTrabajo;
using ECO.Dominio.Servicios.Implementaciones;
using ECO.Dominio.Servicios.Interfaces;
using ECO.Dtos;
using ECO.Dtos.AppSettings;
using ECO.Infraestructura.Aplicacion.ServiciosExternos;
using ECO.Infraestructura.Aplicacion.ServiciosExternos.Config;
using ECO.Infraestructura.Dominio.Repositorio;
using ECO.Infraestructura.Dominio.Repositorio.UnidadTrabajo;
using ECO.Infraestructura.Mapeo;
using Hangfire;
using Hangfire.MySql;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de log4net
var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
builder.Services.AddLogging(loggingBuilder => {loggingBuilder.AddLog4Net();});

// Configuracion de JWT
var configuracionJWT = builder.Configuration.GetSection("JWT");
var issuer = configuracionJWT["Emisor"];
var audience = configuracionJWT["Audiencia"];
var key = configuracionJWT["Llave"];
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer
    (opcion =>
    {
        opcion.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            ClockSkew = TimeSpan.Zero //No se permite tolerancia de tiempo una vez el token caduca (por defecto es 5 minutos si no se establece)
        };
    });

//Mapperly
builder.Services.AddSingleton<IMapperPerfiles, MapperPerfiles>();

builder.Services.AddScoped<ICorreoRepositorio, CorreoRepositorio>();
builder.Services.AddScoped<ICorreoServicio, CorreoServicio>();

builder.Services.AddScoped<ICorreoDestinatarioRepositorio, CorreoDestinatarioRepositorio>();
builder.Services.AddScoped<ICorreoAdjuntoRepositorio, CorreoAdjuntoRepositorio>();
builder.Services.AddScoped<ICorreoEmlRepositorio, CorreoEmlRepositorio>();
builder.Services.AddScoped<IConfiguracionRepositorio, ConfiguracionRepositorio>();
builder.Services.AddScoped<IPlantillaRepositorio, PlantillaRepositorio>();

builder.Services.AddScoped<IColaSolicitudRepositorio, ColaSolicitudRepositorio>();
builder.Services.AddScoped<IColaSolicitudServicio, ColaSolicitudServicio>();
builder.Services.AddScoped<IColaSolicitudValidador, ColaSolicitudValidador>();
builder.Services.AddScoped<IUnidadDeTrabajo, UnidadDeTrabajoEF>();

builder.Services.AddScoped<IConfiguracionServicio, ConfiguracionServicio>();
builder.Services.AddScoped<IPlantillaServicio, PlantillaServicio>();

//Servicio que obtiene el UsuarioId del Token
builder.Services.AddScoped<IUsuarioContextoServicio, UsuarioContextoServicio>();
builder.Services.AddScoped(typeof(IEntidadValidador<>), typeof(EntidadValidador<>));
builder.Services.AddSingleton<ISerializadorJsonServicio, SerializadorJsonServicio>();

builder.Services.AddScoped<IProcesadorTransacciones, ProcesadorTransacciones>();

builder.Services.AddScoped<IEnvioCorreoServicio, EnvioCorreoServicio>();
builder.Services.AddSingleton<IApiResponse, ApiResponse>();

#region REG_Servicios de configuraciones Appsettings

builder.Services.Configure<TrabajosColasSettings>(builder.Configuration.GetSection("TrabajosColas"));
builder.Services.Configure<TrazabilidadCorreoSettings>(builder.Configuration.GetSection("NivelTrazabilidadCorreo"));
builder.Services.Configure<ConfiguracionCorreoSettings>(builder.Configuration.GetSection("ConfiguracionCorreo"));
builder.Services.AddSingleton<IAppSettings, AppSettings>();

#endregion

builder.Services.AddDbContext<AppDbContext>
    (opciones => opciones
    .UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    //ServerVersion.Parse("8.0.39-mysql")
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));


builder.Services.AddHangfire(opciones =>
{
    opciones.UseStorage(
        new MySqlStorage(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            new MySqlStorageOptions { TablesPrefix = "XHAF_ECO_" }));
});

//Necesario para correr el background job server
builder.Services.AddHangfireServer(opciones => { opciones.ServerName = "MSEmpresasServer"; });

builder.Services.AddHttpContextAccessor();

var app = builder.Build();


//Dashboard para ver los jobs en el navegador
app.UseHangfireDashboard("/hangfire");


//Configuracion para la tarea Job en segundo plano que rastrea las solicitudes pendientes de procesar.
var configuracionTrabajosColas = app.Services.GetRequiredService<IAppSettings>();
RecurringJob.AddOrUpdate<IColaSolicitudServicio>("procesador_solicitudes", x => x.ProcesarColaSolicitudesAsync(),
    configuracionTrabajosColas.ObtenerTrabajosColasSettings().ProcesarColaSolicitudesCron);


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
