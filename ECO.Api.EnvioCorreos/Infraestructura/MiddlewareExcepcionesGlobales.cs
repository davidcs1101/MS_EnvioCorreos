using Newtonsoft.Json;
using ECO.Dtos;
using System;
using System.Net;
using Utilidades;
using System.Net.Mail;

namespace ECO.Api.EnvioCorreos.Infraestructura
{
    public class MiddlewareExcepcionesGlobales
    {
        private readonly RequestDelegate _requestDelegate;

        public MiddlewareExcepcionesGlobales(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
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
            var respuesta = new ApiResponse<string>
            {
                Correcto = false,
                Mensaje = Textos.Generales.MENSAJE_ERROR_SERVIDOR
            };

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
