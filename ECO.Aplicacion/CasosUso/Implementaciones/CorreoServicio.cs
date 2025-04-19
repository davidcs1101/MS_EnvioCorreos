using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using ECO.Dtos;
using Utilidades;
using ECO.Aplicacion.CasosUso.Interfaces;
using ECO.Aplicacion.Servicios.Interfaces;

namespace ECO.Aplicacion.CasosUso.Implementaciones
{
    public class CorreoServicio : ICorreoServicio
    {
        private readonly IConfiguration _configuracion;
        private readonly IApiResponse _apiResponse;

        public CorreoServicio(IConfiguration configuration, IApiResponse apiResponse)
        {
            _configuracion = configuration;
            _apiResponse = apiResponse;
        }

        public async Task<ApiResponse<string>> EnviarCorreoAsync(DatoCorreoDto datosCorreoDto)
        {
            Logs.EscribirLog("i","Inicia envío mensajes de correo");
            // Obtener la configuración del correo desde appsettings.json o una fuente similar
            var configuracion = _configuracion.GetSection("ConfiguracionCorreo").GetChildren();
            var usuario = configuracion.Where(x => x.Key == "Usuario").FirstOrDefault()?.Value;
            var clave = configuracion.Where(x => x.Key == "Clave").FirstOrDefault()?.Value;
            var host = configuracion.Where(x => x.Key == "Host").FirstOrDefault()?.Value;
            var puerto = configuracion.Where(x => x.Key == "Puerto").FirstOrDefault()?.Value;
            var usaSsl = configuracion.Where(x => x.Key == "UsaSsl").FirstOrDefault()?.Value;
            var usaCredencialPorDefecto = configuracion.Where(x => x.Key == "UsaCredencialPorDefecto").FirstOrDefault()?.Value;
            var CorreoRespuesta = configuracion.Where(x => x.Key == "CorreoRespuesta").FirstOrDefault()?.Value;


            if (
                string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(clave) ||
                string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(puerto) ||
                string.IsNullOrWhiteSpace(usaSsl) || string.IsNullOrWhiteSpace(usaCredencialPorDefecto) ||
                string.IsNullOrWhiteSpace(CorreoRespuesta)
               )
            {
                Logs.EscribirLog("e", Textos.Generales.MENSAJE_CORREO_CONFIGURACION_ERROR);
                return new ApiResponse<string> { Correcto = false, Mensaje = Textos.Generales.MENSAJE_CORREO_CONFIGURACION_ERROR };
            }

            CorreoRespuesta = string.IsNullOrWhiteSpace(datosCorreoDto.CorreoRespuesta) ? CorreoRespuesta : datosCorreoDto.CorreoRespuesta;
            using (var mensaje = new MailMessage())
            {
                mensaje.From = new MailAddress(usuario, CorreoRespuesta);
                AgregarDestinatarios(mensaje, datosCorreoDto);
                mensaje.Subject = datosCorreoDto.Asunto;
                mensaje.Body = datosCorreoDto.Cuerpo;
                mensaje.IsBodyHtml = datosCorreoDto.esCuerpoHtml;
                AgregarAdjuntos(mensaje, datosCorreoDto.ArchivosAdjuntos);

                using (var smtpClient = new SmtpClient(host, Convert.ToInt32(puerto)))
                {
                    smtpClient.Credentials = new NetworkCredential(usuario, clave);
                    smtpClient.EnableSsl = usaSsl.ToUpper() == "S" ? true : false;
                    smtpClient.UseDefaultCredentials = usaCredencialPorDefecto.ToUpper() == "S" ? true : false;

                    await smtpClient.SendMailAsync(mensaje);
                    Logs.EscribirLog("i", Textos.Generales.MENSAJE_CORREO_ENVIADO_OK);

                    return _apiResponse.CrearRespuesta(true, Textos.Generales.MENSAJE_CORREO_ENVIADO_OK,"");
                }
            }
        }

        private void AgregarDestinatarios(MailMessage mensaje, DatoCorreoDto datosCorreoDto)
        {
            foreach (var item in datosCorreoDto.Destinatarios)
                mensaje.To.Add(item);

            foreach (var item in datosCorreoDto.CC)
                mensaje.CC.Add(item);

            foreach (var item in datosCorreoDto.CCO)
                mensaje.Bcc.Add(item);
        }

        private void AgregarAdjuntos(MailMessage mensaje, IEnumerable<ArchivoAdjuntoDto> archivosAdjuntos)
        {
            foreach (var adjuntoB64 in archivosAdjuntos)
            {
                var adjunto = Convert.FromBase64String(adjuntoB64.Contenido);
                var memoryStream = new MemoryStream(adjunto);
                var attachment = new Attachment(memoryStream, $"{adjuntoB64.Nombre}.{adjuntoB64.Extension}", "application/octet-stream");
                mensaje.Attachments.Add(attachment);
            }
        }
    }


}