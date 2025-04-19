using Newtonsoft.Json;
using ECO.Dtos;
using System.Net;
using Utilidades;
using System.Net.Mail;
using ECO.Aplicacion.Servicios.Interfaces;

namespace ECO.Api.EnvioCorreos.Middlewares
{
    public class MiddlewareExcepcionesGlobales
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly IServiceProvider _serviceProvider;

        public MiddlewareExcepcionesGlobales(RequestDelegate requestDelegate, IServiceProvider serviceProvider)
        {
            _requestDelegate = requestDelegate;
            _serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext httpContext) 
        {
            try
            {
                //Llamamos al siguiente MiddleWare en la cadena de ejecución
                await _requestDelegate(httpContext);
            }
            catch (Exception e)
            {
                await ManejarExcepcionesAsync(httpContext, e);
            }
        }

        private Task ManejarExcepcionesAsync(HttpContext contexto, Exception e) 
        {
            contexto.Response.ContentType = "application/json";
            using (var scope = _serviceProvider.CreateScope()) 
            {
                var _apiResponse = scope.ServiceProvider.GetRequiredService<IApiResponse>();
                var respuesta = _apiResponse.CrearRespuesta(false, Textos.Generales.MENSAJE_ERROR_SERVIDOR, "");

                if (e is SmtpException)
                {
                    contexto.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                    respuesta.Mensaje = e.Message;
                }
                else
                {
                    contexto.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    Logs.EscribirLog("e", "", e);
                }

                // Si es desarrollo, incluir el detalle del error
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    //respuesta.Mensaje = e.Message;
                }

                var respuestaJson = JsonConvert.SerializeObject(respuesta);
                return contexto.Response.WriteAsync(respuestaJson);
            }
        }
    }
}
