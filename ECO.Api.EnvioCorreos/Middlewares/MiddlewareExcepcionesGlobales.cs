using ECO.Aplicacion.Servicios.Implementaciones;
using ECO.Aplicacion.Servicios.Interfaces;
using ECO.Aplicacion.ServiciosExternos;
using ECO.Dominio.Excepciones;
using ECO.Dtos;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;
using Utilidades;

namespace ECO.Api.EnvioCorreos.Middlewares
{
    public class MiddlewareExcepcionesGlobales
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly ISerializadorJsonServicio _serializadorJsonServicio;
        private readonly IApiResponse _apiResponse;

        public MiddlewareExcepcionesGlobales(RequestDelegate requestDelegate, IServiceProvider serviceProvider, IApiResponse apiResponse, ISerializadorJsonServicio serializadorJsonServicio)
        {
            _requestDelegate = requestDelegate;
            _apiResponse = apiResponse;
            _serializadorJsonServicio = serializadorJsonServicio;
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
            var respuesta = _apiResponse.CrearRespuesta(false, Textos.Generales.MENSAJE_ERROR_SERVIDOR, "");

            if (e is DatoNoEncontradoException)
            {
                contexto.Response.StatusCode = (int)HttpStatusCode.NotFound;
                respuesta.Mensaje = e.Message;
            }
            else if (e is DatoYaExisteException)
            {
                contexto.Response.StatusCode = (int)HttpStatusCode.Conflict;
                respuesta.Mensaje = e.Message;
            }
            else if (e is SolicitudHttpException)
            {
                contexto.Response.StatusCode = (int)HttpStatusCode.BadGateway;
                respuesta.Mensaje = e.Message;
            }
            else if (e is SmtpException)
            {
                contexto.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                respuesta.Mensaje = e.Message;
            }
            else if (e is DatoInactivoException)
            {
                contexto.Response.StatusCode = (int)HttpStatusCode.Conflict;
                respuesta.Mensaje = e.Message;
            }
            else
            {
                contexto.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            //Siempre escribimos en los logs las diferentes Excepciones
            Logs.EscribirLog("e", "", e);

            // Si es desarrollo, incluir el detalle del error
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                //respuesta.Mensaje = e.Message;
            }

            var respuestaJson = _serializadorJsonServicio.Serializar(respuesta);
            return contexto.Response.WriteAsync(respuestaJson);
        }
    }
}
