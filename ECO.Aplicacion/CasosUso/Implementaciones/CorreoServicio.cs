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
        private readonly IUsuarioContextoServicio _usuarioContextoServicio;

        private readonly IConfiguracionesTrazabilidadCorreo _configuracionesTrazabilidadCorreo;


        public CorreoServicio(ICorreoRepositorio correoRepositorio, ICorreoDestinatarioRepositorio correoDestinatarioRepositorio, ICorreoAdjuntoRepositorio correoAdjuntoRepositorio, IColaSolicitudRepositorio colaSolicitudRepositorio, IMapperPerfiles mapper, IApiResponse apiResponse, IUnidadDeTrabajo unidadDeTrabajo, IProcesadorTransacciones procesadorTransacciones, ISerializadorJsonServicio serializadorJsonServicio, IConfiguracionesTrazabilidadCorreo configuracionesTrazabilidadCorreo, IUsuarioContextoServicio usuarioContextoServicio)
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
            _configuracionesTrazabilidadCorreo = configuracionesTrazabilidadCorreo;
            _usuarioContextoServicio = usuarioContextoServicio;
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
                correo.EmpresaId = _usuarioContextoServicio.ObtenerEmpresaIdToken();

                _correoRepositorio.MarcarCrear(correo);
                await _unidadDeTrabajo.GuardarCambiosAsync();
                id = correo.Id;

                if (_configuracionesTrazabilidadCorreo.ObtenerGuardarDetalleCorreo() || 
                datosCorreoRequest.Acciones.GuardarDetalleCorreo)
                {
                    //Creamos ECO_CorreoDestinatarios
                    CrearDestinatarios(id, datosCorreoRequest.Destinatarios, TipoDestinatario.Para);
                    CrearDestinatarios(id, datosCorreoRequest.CC, TipoDestinatario.CC);
                    CrearDestinatarios(id, datosCorreoRequest.CCO, TipoDestinatario.CCO);
                }

                if (_configuracionesTrazabilidadCorreo.ObtenerGuardarAdjuntosCorreo() || 
                datosCorreoRequest.Acciones.GuardarAdjuntosCorreo)
                {
                    //Creamos ECO_CorreoAdjuntos
                    CrearAdjuntos(id, datosCorreoRequest.ArchivosAdjuntos);
                }

                var guardarEmlCorreo =
                    _configuracionesTrazabilidadCorreo.ObtenerGuardarEmlCorreo() ||
                    datosCorreoRequest.Acciones.GuardarEmlCorreo;


                var datosCorreo = _mapper.DatoCorreoRequestADatoCorreoDto(datosCorreoRequest);
                datosCorreo.CorreoId = id;
                datosCorreo.GuardarEmlCorreo = guardarEmlCorreo;
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
                Intentos = 0,
                FechaCreado = DateTime.Now
            };
            _colaSolicitudRepositorio.MarcarCrear(solicitud);
            return solicitud;
        }

        private void CrearDestinatarios(int correoId,List<string> destinatarios, TipoDestinatario tipoDestinatario) 
        {
            foreach (var destinatario in destinatarios)
            {
                var correoDestinatario = new ECO_CorreoDestinatario();
                correoDestinatario.CorreoId = correoId;
                correoDestinatario.Destinatario = destinatario;
                correoDestinatario.Tipo = tipoDestinatario;

                _correoDestinatarioRepositorio.MarcarCrear(correoDestinatario);
            }
        }

        private void CrearAdjuntos(int correoId, List<ArchivoAdjuntoDto> adjuntosB64) 
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

                _correoAdjuntoRepositorio.MarcarCrear(correoAdjunto);
            }
        }
    }
}
