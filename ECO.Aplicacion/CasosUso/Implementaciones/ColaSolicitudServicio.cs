using ECO.Aplicacion.CasosUso.Interfaces;
using ECO.Aplicacion.Servicios.Interfaces;
using ECO.Aplicacion.ServiciosExternos;
using ECO.Aplicacion.ServiciosExternos.config;
using ECO.Dominio.Entidades;
using ECO.Dominio.Enumeraciones;
using ECO.Dominio.Repositorio;
using ECO.Dominio.Repositorio.UnidadTrabajo;
using ECO.Dominio.Servicios.Interfaces;
using ECO.Dtos;
using Utilidades;

namespace ECO.Aplicacion.CasosUso.Implementaciones
{
    public class ColaSolicitudServicio : IColaSolicitudServicio
    {
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;
        private readonly IColaSolicitudRepositorio _colaSolicitudRepositorio;
        private readonly ISerializadorJsonServicio _serializadorJsonServicio;
        private readonly IColaSolicitudValidador _colaSolicitudValidador;
        private readonly IAppSettings _appSettings;
        private readonly IEnvioCorreoServicio _envioCorreoServicio;
        private readonly ICorreoEmlRepositorio _correoEmlRepositorio;
        private readonly ICorreoRepositorio _correoRepositorio;

        public ColaSolicitudServicio(IUnidadDeTrabajo unidadDeTrabajo, IColaSolicitudRepositorio colaSolicitudRepositorio, ISerializadorJsonServicio serializadorJsonServicio, IColaSolicitudValidador colaSolicitudValidador, IEnvioCorreoServicio envioCorreoServicio, IAppSettings appSettings, ICorreoEmlRepositorio correoEmlRepositorio, ICorreoRepositorio correoRepositorio)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _colaSolicitudRepositorio = colaSolicitudRepositorio;
            _serializadorJsonServicio = serializadorJsonServicio;
            _colaSolicitudValidador = colaSolicitudValidador;
            _envioCorreoServicio = envioCorreoServicio;
            _appSettings = appSettings;
            _correoEmlRepositorio = correoEmlRepositorio;
            _correoRepositorio = correoRepositorio;
        }

        public async Task ProcesarColaSolicitudesAsync()
        {
            var cantidadRegistrosProcesar = _appSettings.ObtenerTrabajosColasSettings().CantidadRegistrosProcesarIteracion;
            var pendientes = _colaSolicitudRepositorio.Listar().
                Where(c => c.Estado == EstadoCola.Pendiente).OrderBy(c => c.Id)
                .Take(cantidadRegistrosProcesar).ToList();

            foreach (var solicitud in pendientes)
            {
                await this.ProcesarPorColaSolicitudIdAsync(solicitud.Id);
            }
        }

        public async Task ProcesarPorColaSolicitudIdAsync(int id, bool validarEstadoPendiente = false)
        {
            await using var transaccion = await _unidadDeTrabajo.IniciarTransaccionAsync();
            var cantidadIntentos = _appSettings.ObtenerTrabajosColasSettings().CantidadIntentosPorRegistroEnCola;
            var solicitudExiste = await _colaSolicitudRepositorio.ObtenerPorIdAsync(id);
            _colaSolicitudValidador.ValidarDatoNoEncontrado(solicitudExiste, Textos.ColasSolicitudes.MENSAJE_COLASOLICITUD_NO_EXISTE_ID);

            if (validarEstadoPendiente)
            {
                if (solicitudExiste.Estado != EstadoCola.Pendiente)
                {
                    Logs.EscribirLog("w", $"{Textos.ColasSolicitudes.MENSAJE_COLASOLICITUD_YA_PROCESADA}: {solicitudExiste.Id}");
                    return;
                }
            }

            try
            {
                solicitudExiste.Estado = EstadoCola.Procesando;
                solicitudExiste.FechaUltimoIntento = DateTime.Now;
                _colaSolicitudRepositorio.MarcarModificar(solicitudExiste);
                await _unidadDeTrabajo.GuardarCambiosAsync();

                switch (solicitudExiste.Tipo)
                {
                    case EventosColas.ENVIARCORREO:
                        await ProcesarCorreo(solicitudExiste.Payload);
                        break;
                }

                solicitudExiste.Estado = EstadoCola.Exitoso;
                solicitudExiste.ErrorMensaje = null;
            }
            catch (Exception ex)
            {
                solicitudExiste.Intentos++;
                solicitudExiste.Estado = solicitudExiste.Intentos >= cantidadIntentos ? EstadoCola.Fallido : EstadoCola.Pendiente;
                solicitudExiste.ErrorMensaje = ex.Message;
                Logs.EscribirLog("e", $"{Textos.ColasSolicitudes.MENSAJE_COLASOLICITUD_ERROR_PROCESO} : {solicitudExiste.Id}", ex);
            }
            _colaSolicitudRepositorio.MarcarModificar(solicitudExiste);
            await _unidadDeTrabajo.GuardarCambiosAsync();
            await transaccion.CommitAsync();
        }



        private async Task CrearEml(int correoId, byte[] contenidoEml, int usuarioCreadorId = 1)
        {
            var correoEml = new ECO_CorreoEml
            {
                CorreoId = correoId,
                Nombre = $"Correo_{correoId}.eml",
                TamanoBytes = contenidoEml.Length,
                ContenidoArchivo = contenidoEml,
                UsuarioCreadorId = usuarioCreadorId
            };

            await _correoEmlRepositorio.CrearAsync(correoEml);
        }
        private async Task ActualizarCorreo(int correoId, EstadoCorreo estado, string? errorMensaje = null)
        {
            var correo = await _correoRepositorio.ObtenerPorIdAsync(correoId);

            correo.Estado = estado;
            correo.FechaEnvio = estado == EstadoCorreo.Enviado ? DateTime.UtcNow : null;
            correo.ErrorMensaje = errorMensaje;

            await _correoRepositorio.ModificarAsync(correo);
        }
        private async Task ProcesarCorreo(string payload) 
        {
            EstadoCorreo estado = EstadoCorreo.Enviado;
            string? mensaje = "";
            var datosCorreoDto = _serializadorJsonServicio.Deserializar<DatosCorreoRequest>(payload);
            int correoId = datosCorreoDto.CorreoId;

            try
            {
                var respuesta = await _envioCorreoServicio.EnviarCorreoAsync(datosCorreoDto);
                if (respuesta.Correcto)
                {
                    var eml = respuesta.Data;
                    if (eml != null)
                        await CrearEml(correoId, eml, 1);
                    await ActualizarCorreo(correoId, estado);
                }
                else
                {
                    mensaje = respuesta.Mensaje;
                    estado = EstadoCorreo.Fallido;
                    await ActualizarCorreo(correoId, estado, mensaje);
                    throw new Exception(mensaje);
                }
            }
            catch (Exception e)
            {
                mensaje = e.ToString();
                estado = EstadoCorreo.Fallido;
                await ActualizarCorreo(correoId, estado, mensaje);
                throw;
            }
        }
    }
}
