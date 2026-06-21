using ECO.Aplicacion.Servicios.Interfaces;
using ECO.Aplicacion.ServiciosExternos;
using ECO.Aplicacion.ServiciosExternos.config;
using ECO.Dominio.Entidades;
using ECO.Dominio.Enumeraciones;
using ECO.Dominio.Repositorio;
using ECO.Dtos;
using ECO.Dtos.AppSettings;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Utils;
using System.Net;
using System.Net.Mail;
using Utilidades;

namespace ECO.Infraestructura.Aplicacion.ServiciosExternos
{
    public class EnvioCorreoServicio : IEnvioCorreoServicio
    {
        private readonly IAppSettings _appSettings;
        private readonly IApiResponse _apiResponse;
        private readonly ICorreoConfiguracionRepositorio _correoConfiguracionRepositorio;

        public EnvioCorreoServicio(IAppSettings appSettings, IApiResponse apiResponse, ICorreoConfiguracionRepositorio correoConfiguracionRepositorio)
        {
            _appSettings = appSettings;
            _apiResponse = apiResponse;
            _correoConfiguracionRepositorio = correoConfiguracionRepositorio;
        }

        public async Task<ApiResponse<byte[]?>> EnviarCorreoAsync(DatosCorreoRequest datosCorreoDto)
        {
            Logs.EscribirLog("i","Inicia envío mensajes de correo");
            // Obtener la configuración del correo.
            var settings = await ObtenerConfiguracionCorreo(datosCorreoDto.EmpresaId, datosCorreoDto.CodigoConfiguracionEnvio);
            var usuario = settings.Usuario;
            var clave = settings.Clave;
            var host = settings.Host;
            var puerto = settings.Puerto;
            var usaSsl = settings.UsaSsl;
            var usaCredencialPorDefecto = settings.UsaCredencialPorDefecto;
            var correoRespuesta = settings.CorreoRespuesta;
            
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(clave) ||
                string.IsNullOrWhiteSpace(host) || puerto == 0 || string.IsNullOrWhiteSpace(correoRespuesta))
            {
                Logs.EscribirLog("e", Textos.Generales.MENSAJE_CORREO_CONFIGURACION_ERROR);
                return _apiResponse.CrearRespuesta<byte[]?>(false, Textos.Generales.MENSAJE_CORREO_CONFIGURACION_ERROR, null);
            }

            correoRespuesta = string.IsNullOrWhiteSpace(datosCorreoDto.CorreoRespuesta) ? correoRespuesta : datosCorreoDto.CorreoRespuesta;

            using (var mensaje = new MailMessage())
            {
                mensaje.From = new MailAddress(usuario, correoRespuesta);
                AgregarDestinatarios(mensaje, datosCorreoDto);
                mensaje.Subject = datosCorreoDto.Asunto;
                mensaje.Body = datosCorreoDto.Cuerpo;
                mensaje.IsBodyHtml = datosCorreoDto.EsCuerpoHtml;
                AgregarAdjuntos(mensaje, datosCorreoDto.ArchivosAdjuntos);

                byte[] eml = null;
                if (datosCorreoDto.Acciones.GuardarEmlCorreo)
                    eml = GenerarEml(datosCorreoDto, usuario, correoRespuesta);

                using (var smtpClient = new SmtpClient(host, puerto))
                {
                    smtpClient.Credentials = new NetworkCredential(usuario, clave);
                    smtpClient.EnableSsl = usaSsl;
                    smtpClient.UseDefaultCredentials = usaCredencialPorDefecto;

                    await smtpClient.SendMailAsync(mensaje);
                    Logs.EscribirLog("i", Textos.Generales.MENSAJE_CORREO_ENVIADO_OK);
                }

                return _apiResponse.CrearRespuesta<byte[]?>(true,Textos.Generales.MENSAJE_CORREO_ENVIADO_OK, eml);

            }
        }

        private void AgregarDestinatarios(MailMessage mensaje, DatosCorreoRequest datosCorreoDto)
        {
            foreach (var item in datosCorreoDto.Destinatarios)
                mensaje.To.Add(item);

            foreach (var item in datosCorreoDto.CC)
                mensaje.CC.Add(item);

            foreach (var item in datosCorreoDto.CCO)
                mensaje.Bcc.Add(item);
        }
        private void AgregarAdjuntos(MailMessage mensaje, IEnumerable<CorreoAdjuntoRequest> archivosAdjuntos)
        {
            foreach (var adjuntoB64 in archivosAdjuntos)
            {
                var adjunto = Convert.FromBase64String(adjuntoB64.Contenido);
                var memoryStream = new MemoryStream(adjunto);
                var attachment = new Attachment(memoryStream, $"{adjuntoB64.Nombre}{adjuntoB64.Extension}", "application/octet-stream");
                mensaje.Attachments.Add(attachment);
            }
        }
        private byte[] GenerarEml(DatosCorreoRequest datosCorreoDto, string usuario, string correoRespuesta)
        {
            var mensaje = new MimeMessage();

            // Dirección real del remitente SMTP
            mensaje.From.Add(new MailboxAddress(correoRespuesta, usuario));

            // Reply-To únicamente si viene informado
            if (!string.IsNullOrWhiteSpace(datosCorreoDto.CorreoRespuesta))
            {
                mensaje.ReplyTo.Add(
                    MailboxAddress.Parse(datosCorreoDto.CorreoRespuesta));
            }

            foreach (var destinatario in datosCorreoDto.Destinatarios)
                mensaje.To.Add(MailboxAddress.Parse(destinatario));

            foreach (var destinatario in datosCorreoDto.CC)
                mensaje.Cc.Add(MailboxAddress.Parse(destinatario));

            foreach (var destinatario in datosCorreoDto.CCO)
                mensaje.Bcc.Add(MailboxAddress.Parse(destinatario));

            mensaje.Subject = datosCorreoDto.Asunto;

            var bodyBuilder = new BodyBuilder();

            if (datosCorreoDto.EsCuerpoHtml)
            {
                bodyBuilder.HtmlBody = datosCorreoDto.Cuerpo;

                // Opcional pero recomendable:
                // genera versión texto plano automáticamente
                //bodyBuilder.TextBody = MimeKit.Text.TextConverter
                //    .ConvertToPlainText(datosCorreoDto.Cuerpo);
            }
            else
            {
                bodyBuilder.TextBody = datosCorreoDto.Cuerpo;
            }

            foreach (var adjunto in datosCorreoDto.ArchivosAdjuntos)
            {
                var bytes = Convert.FromBase64String(adjunto.Contenido);

                var nombreArchivo = string.IsNullOrWhiteSpace(adjunto.Extension)
                    ? adjunto.Nombre
                    : $"{adjunto.Nombre}{adjunto.Extension}";

                bodyBuilder.Attachments.Add(nombreArchivo, bytes);
            }

            mensaje.Date = DateTimeOffset.UtcNow;
            mensaje.MessageId = MimeUtils.GenerateMessageId();

            mensaje.Body = bodyBuilder.ToMessageBody();

            using var stream = new MemoryStream();

            mensaje.WriteTo(stream);

            return stream.ToArray();
        }
        private async Task<ConfiguracionCorreoSettings> ObtenerConfiguracionCorreo(int? empresaId, string? codigoConfiguracionEnvio)
        {
            var settings = _appSettings.ObtenerConfiguracionCorreoSettings();
            ECO_CorreoConfiguracion? configuracionEmpresa = null;

            if (empresaId is not null)
            {
                string codigoConfiguracion = "PRINCIPAL";
                if (!string.IsNullOrWhiteSpace(codigoConfiguracionEnvio))
                    codigoConfiguracion = codigoConfiguracionEnvio;
                
                configuracionEmpresa = await _correoConfiguracionRepositorio.ObtenerPorEmpresaIdYCodigoAsync(empresaId.Value, codigoConfiguracion);

                if (configuracionEmpresa is not null)
                    settings = new ConfiguracionCorreoSettings()
                    {
                        Usuario = configuracionEmpresa.Usuario,
                        Clave = configuracionEmpresa.Clave,
                        Host = configuracionEmpresa.Host,
                        Puerto = configuracionEmpresa.Puerto,
                        UsaSsl = configuracionEmpresa.UsaSsl,
                        UsaCredencialPorDefecto = configuracionEmpresa.UsaCredencialPorDefecto,
                        CorreoRespuesta = configuracionEmpresa.CorreoRespuesta
                    };
            }
            return settings;
        }
    }
}