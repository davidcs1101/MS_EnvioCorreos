using ECO.Aplicacion.CasosUso.Interfaces;
using ECO.Aplicacion.ServiciosExternos;
using ECO.Aplicacion.ServiciosExternos.Mapeo;
using ECO.Aplicacion.Servicios.Interfaces;
using ECO.Dtos;
using ECO.Dominio.Repositorio;
using ECO.Dominio.Repositorio.UnidadTrabajo;
using ECO.Dominio.Enumeraciones;
using Utilidades;
using ECO.Aplicacion.ServiciosExternos.config;
using ECO.Dominio.Entidades;


namespace ECO.Aplicacion.CasosUso.Implementaciones
{
    public class CorreoServicio : ICorreoServicio
    {
        private readonly ICorreoRepositorio _correoRepositorio;
        private readonly ICorreoDestinatarioRepositorio _correoDestinatarioRepositorio;
        private readonly ICorreoAdjuntoRepositorio _correoAdjuntoRepositorio;
        private readonly IColaSolicitudRepositorio _colaSolicitudRepositorio;

        private readonly IMapperPerfiles _mapper;
        private readonly IApiResponse _apiResponse;

        private readonly IUnidadDeTrabajo _unidadDeTrabajo;
        private readonly IProcesadorTransacciones _procesadorTransacciones;
        private readonly ISerializadorJsonServicio _serializadorJsonServicio;
        //private readonly IUsuarioContextoServicio _usuarioContextoServicio;

        private readonly IAppSettings _appSettings;


        public CorreoServicio(ICorreoRepositorio correoRepositorio, ICorreoDestinatarioRepositorio correoDestinatarioRepositorio, ICorreoAdjuntoRepositorio correoAdjuntoRepositorio, IColaSolicitudRepositorio colaSolicitudRepositorio, IMapperPerfiles mapper, IApiResponse apiResponse, IUnidadDeTrabajo unidadDeTrabajo, IProcesadorTransacciones procesadorTransacciones, ISerializadorJsonServicio serializadorJsonServicio, IUsuarioContextoServicio usuarioContextoServicio, IAppSettings appSettings)
        {
            _correoRepositorio = correoRepositorio;
            _correoDestinatarioRepositorio = correoDestinatarioRepositorio;
            _correoAdjuntoRepositorio = correoAdjuntoRepositorio;
            _colaSolicitudRepositorio = colaSolicitudRepositorio;
            _mapper = mapper;
            _apiResponse = apiResponse;
            _unidadDeTrabajo = unidadDeTrabajo;
            _procesadorTransacciones = procesadorTransacciones;
            _serializadorJsonServicio = serializadorJsonServicio;
            _appSettings = appSettings;
            //_usuarioContextoServicio = usuarioContextoServicio;
        }

        public async Task<ApiResponse<int>> CrearAsync(DatosCorreoRequest datosCorreoRequest)
        {
            var id = 0;
            var cola = new ECO_ColaSolicitud();
            await _procesadorTransacciones.EjecutarEnTransaccionAsync(async () =>
            {
                //Creamos ECO_Correo
                var correo = _mapper.DatoCorreoRequestACorreo(datosCorreoRequest);
                correo.Estado = EstadoCorreo.Pendiente;

                if (datosCorreoRequest is DatosCorreoEmpresaRequest empresaRequest)
                    correo.EmpresaId = empresaRequest.EmpresaId;

                _correoRepositorio.MarcarCrear(correo);
                await _unidadDeTrabajo.GuardarCambiosAsync();
                id = correo.Id;
                var usuarioId = correo.UsuarioCreadorId;

                //Creamos ECO_CorreoDestinatarios
                if (_appSettings.ObtenerGuardarDetalleCorreo() || 
                datosCorreoRequest.Acciones.GuardarDetalleCorreo)
                {
                    CrearDestinatarios(id, datosCorreoRequest.Destinatarios, TipoDestinatario.Para, usuarioId);
                    CrearDestinatarios(id, datosCorreoRequest.CC, TipoDestinatario.CC, usuarioId);
                    CrearDestinatarios(id, datosCorreoRequest.CCO, TipoDestinatario.CCO, usuarioId);
                }

                //Creamos ECO_CorreoAdjuntos
                if (_appSettings.ObtenerGuardarAdjuntosCorreo() || 
                datosCorreoRequest.Acciones.GuardarAdjuntosCorreo)
                    CrearAdjuntos(id, datosCorreoRequest.ArchivosAdjuntos, usuarioId);

                datosCorreoRequest.Acciones.GuardarEmlCorreo =
                    _appSettings.ObtenerGuardarEmlCorreo() ||
                    datosCorreoRequest.Acciones.GuardarEmlCorreo;


                var datosCorreo = _mapper.DatoCorreoRequestADatoCorreoDto(datosCorreoRequest);
                datosCorreo.CorreoId = id;
                cola = this.AgregarColaSolicitud(datosCorreo);

                await _unidadDeTrabajo.GuardarCambiosAsync();

            });

            return _apiResponse.CrearRespuesta(true, Textos.Generales.MENSAJE_REGISTRO_CREADO, id);
        }

        private ECO_ColaSolicitud AgregarColaSolicitud(DatosCorreoDto datoCorreoDto)
        {
            var solicitud = new ECO_ColaSolicitud
            {
                Tipo = EventosColas.ENVIARCORREO,
                Payload = _serializadorJsonServicio.Serializar(datoCorreoDto),
                Estado = EstadoCola.Pendiente,
                Intentos = 0
            };
            _colaSolicitudRepositorio.MarcarCrear(solicitud);
            return solicitud;
        }

        private void CrearDestinatarios(int correoId,List<string> destinatarios, TipoDestinatario tipoDestinatario, int usuarioCreadorId) 
        {
            foreach (var destinatario in destinatarios)
            {
                var correoDestinatario = new ECO_CorreoDestinatario();
                correoDestinatario.CorreoId = correoId;
                correoDestinatario.Destinatario = destinatario;
                correoDestinatario.Tipo = tipoDestinatario;
                correoDestinatario.UsuarioCreadorId = usuarioCreadorId;

                _correoDestinatarioRepositorio.MarcarCrear(correoDestinatario);
            }
        }

        private void CrearAdjuntos(int correoId, List<ArchivoAdjuntoDto> adjuntosB64, int usuarioCreadorId) 
        {
            foreach (var adjunto in adjuntosB64)
            {
                var bytes = Convert.FromBase64String(adjunto.Contenido);

                var correoAdjunto = new ECO_CorreoAdjunto();
                correoAdjunto.CorreoId = correoId;
                correoAdjunto.Nombre = adjunto.Nombre;
                correoAdjunto.Extension = adjunto.Extension;
                correoAdjunto.TamanoBytes = bytes.Length;
                correoAdjunto.ContenidoArchivo = bytes;
                correoAdjunto.TipoContenido = TiposMime.ObtenerTipoMime(adjunto.Extension);
                correoAdjunto.UsuarioCreadorId = usuarioCreadorId;

                _correoAdjuntoRepositorio.MarcarCrear(correoAdjunto);
            }
        }
    }
}
