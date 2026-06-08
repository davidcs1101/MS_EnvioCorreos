using ECO.Aplicacion.CasosUso.Interfaces;
using ECO.Aplicacion.Servicios.Interfaces;
using ECO.Aplicacion.ServiciosExternos;
using ECO.Aplicacion.ServiciosExternos.config;
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
        private readonly IConfiguracionesTrabajosColas _configuracionesTrabajosColas;
        private readonly IEnvioCorreoServicio _envioCorreoServicio;

        public ColaSolicitudServicio(IUnidadDeTrabajo unidadDeTrabajo, IColaSolicitudRepositorio colaSolicitudRepositorio, ISerializadorJsonServicio serializadorJsonServicio, IColaSolicitudValidador colaSolicitudValidador, IConfiguracionesTrabajosColas configuracionesTrabajosColas, IEnvioCorreoServicio envioCorreoServicio)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _colaSolicitudRepositorio = colaSolicitudRepositorio;
            _serializadorJsonServicio = serializadorJsonServicio;
            _colaSolicitudValidador = colaSolicitudValidador;
            _configuracionesTrabajosColas = configuracionesTrabajosColas;
            _envioCorreoServicio = envioCorreoServicio;
        }

        public async Task ProcesarColaSolicitudesAsync()
        {
            var pendientes = _colaSolicitudRepositorio.Listar().Where(c => c.Estado == EstadoCola.Pendiente).OrderBy(c => c.Id).Take(30).ToList();

            foreach (var solicitud in pendientes)
            {
                await this.ProcesarPorColaSolicitudIdAsync(solicitud.Id);
            }
        }

        public async Task ProcesarPorColaSolicitudIdAsync(int id, bool validarEstadoPendiente = false)
        {
            await using var transaccion = await _unidadDeTrabajo.IniciarTransaccionAsync();

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
                        await _envioCorreoServicio.EnviarCorreoAsync(_serializadorJsonServicio.Deserializar<DatosCorreoDto>(solicitudExiste.Payload));
                        break;
                }

                solicitudExiste.Estado = EstadoCola.Exitoso;
                solicitudExiste.ErrorMensaje = null;
            }
            catch (Exception ex)
            {
                solicitudExiste.Intentos++;
                solicitudExiste.Estado = solicitudExiste.Intentos >= _configuracionesTrabajosColas.ObtenerCantidadIntentosPorRegistroEnCola() ? EstadoCola.Fallido : EstadoCola.Pendiente;
                solicitudExiste.ErrorMensaje = ex.Message;
                Logs.EscribirLog("e", $"{Textos.ColasSolicitudes.MENSAJE_COLASOLICITUD_ERROR_PROCESO} : {solicitudExiste.Id}", ex);
            }
            _colaSolicitudRepositorio.MarcarModificar(solicitudExiste);
            await _unidadDeTrabajo.GuardarCambiosAsync();
            await transaccion.CommitAsync();
        }
    }
}
