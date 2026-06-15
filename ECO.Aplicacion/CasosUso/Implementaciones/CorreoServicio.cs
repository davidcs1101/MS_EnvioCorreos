using ECO.Aplicacion.CasosUso.Interfaces;
using ECO.Aplicacion.Servicios.Interfaces;
using ECO.Aplicacion.ServiciosExternos;
using ECO.Aplicacion.ServiciosExternos.config;
using ECO.Aplicacion.ServiciosExternos.Mapeo;
using ECO.Dominio.Entidades;
using ECO.Dominio.Enumeraciones;
using ECO.Dominio.Repositorio;
using ECO.Dominio.Repositorio.UnidadTrabajo;
using ECO.Dominio.Servicios.Interfaces;
using ECO.Dtos;
using System.Text;
using Utilidades;


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
        private readonly IEntidadValidador<ECO_Correo> _correoValidadorServicio;
        //private readonly IUsuarioContextoServicio _usuarioContextoServicio;

        private readonly IAppSettings _appSettings;


        public CorreoServicio(ICorreoRepositorio correoRepositorio, ICorreoDestinatarioRepositorio correoDestinatarioRepositorio, ICorreoAdjuntoRepositorio correoAdjuntoRepositorio, IColaSolicitudRepositorio colaSolicitudRepositorio, IMapperPerfiles mapper, IApiResponse apiResponse, IUnidadDeTrabajo unidadDeTrabajo, IProcesadorTransacciones procesadorTransacciones, ISerializadorJsonServicio serializadorJsonServicio, IUsuarioContextoServicio usuarioContextoServicio, IAppSettings appSettings, IEntidadValidador<ECO_Correo> correoValidadorServicio)
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
            _correoValidadorServicio = correoValidadorServicio;
            //_usuarioContextoServicio = usuarioContextoServicio;
        }

        public async Task<ApiResponse<int>> CrearAsync(CorreoCreacionRequest datosCorreoRequest)
        {
            var id = 0;
            var cola = new ECO_ColaSolicitud();
            var trazabilidadCorreo = _appSettings.ObtenerTrazabilidadCorreoSettings();
            await _procesadorTransacciones.EjecutarEnTransaccionAsync(async () =>
            {
                //Creamos ECO_Correo
                var correo = _mapper.DatoCorreoRequestACorreo(datosCorreoRequest);
                correo.Estado = EstadoCorreo.Pendiente;

                if (datosCorreoRequest is CorreoEmpresaCreacionRequest empresaRequest)
                    correo.EmpresaId = empresaRequest.EmpresaId;

                _correoRepositorio.MarcarCrear(correo);
                await _unidadDeTrabajo.GuardarCambiosAsync();
                id = correo.Id;
                var usuarioId = correo.UsuarioCreadorId;

                //Creamos ECO_CorreoDestinatarios
                if (trazabilidadCorreo.GuardarDetalleCorreo || 
                datosCorreoRequest.Acciones.GuardarDetalleCorreo)
                {
                    CrearDestinatarios(id, datosCorreoRequest.Destinatarios, TipoDestinatario.Para, usuarioId);
                    CrearDestinatarios(id, datosCorreoRequest.CC, TipoDestinatario.CC, usuarioId);
                    CrearDestinatarios(id, datosCorreoRequest.CCO, TipoDestinatario.CCO, usuarioId);
                }

                //Creamos ECO_CorreoAdjuntos
                if (trazabilidadCorreo.GuardarAdjuntosCorreo || 
                datosCorreoRequest.Acciones.GuardarAdjuntosCorreo)
                    CrearAdjuntos(id, datosCorreoRequest.ArchivosAdjuntos, usuarioId);

                datosCorreoRequest.Acciones.GuardarEmlCorreo =
                    trazabilidadCorreo.GuardarEmlCorreo ||
                    datosCorreoRequest.Acciones.GuardarEmlCorreo;


                var datosCorreo = _mapper.DatoCorreoRequestADatoCorreoDto(datosCorreoRequest);
                datosCorreo.CorreoId = id;
                cola = this.AgregarColaSolicitud(datosCorreo);

                await _unidadDeTrabajo.GuardarCambiosAsync();

            });

            return _apiResponse.CrearRespuesta(true, Textos.Generales.MENSAJE_REGISTRO_CREADO, id);
        }

        public async Task<ApiResponse<CorreoDto?>> ObtenerPorIdYEmpresaIdAsync(int id) 
        {
            var empresaId = 1;//Esto se debe obtener desde el token empresarial   
            var correo = await _correoRepositorio.ObtenerPorIdYEmpresaIdAsync(id, empresaId);
            _correoValidadorServicio.ValidarDatoNoEncontrado(correo, Textos.Correos.MENSAJE_CORREO_NO_EXISTE_ID);

            //var correoDto = _mapper.CorreoACorreoDto(correo);
            var correoDto = new CorreoDto()
            {
                Id = correo.Id,
                Asunto = correo.Asunto,
                Cuerpo = correo.Cuerpo,
                EsCuerpoHtml = correo.EsCuerpoHtml,
                CorreoRespuesta = correo.CorreoRespuesta,
                Estado = correo.Estado,
                NombreEstado = correo.Estado.ToString(),
                ErrorMensaje = correo.ErrorMensaje,
                FechaEnvio = correo.FechaEnvio,
                EmpresaId = correo.EmpresaId,
                UsuarioCreadorId = correo.UsuarioCreadorId,
                FechaCreado = correo.FechaCreado,
            };

            var destinatarios = new List<CorreoDestinatarioDto>();
            var adjuntos = new List<CorreoAdjuntoDto>();
            var eml = new CorreoEmlDto();
            
            foreach (var destinatario in correo.CorreosDestinatarios)
            {
                var destinatarioDto = new CorreoDestinatarioDto()
                {
                    Destinatario = destinatario.Destinatario,
                    Tipo = destinatario.Tipo,
                    NombreTipo = destinatario.Tipo.ToString()
                };
                destinatarios.Add(destinatarioDto);
            }
            correoDto.CorreosDestinatarios = destinatarios;

            foreach (var adjunto in correo.CorreosAdjuntos)
            {
                var adjuntoDto = new CorreoAdjuntoDto()
                {
                    Id = adjunto.Id,
                    Nombre = adjunto.Nombre,
                    Extension = adjunto.Extension,
                    TipoContenido = adjunto.TipoContenido,
                    TamanoBytes = adjunto.TamanoBytes,
                    ContenidoArchivo = adjunto.ContenidoArchivo,
                    ContenidoArchivoB64 = Convert.ToBase64String(adjunto.ContenidoArchivo)
                };
                adjuntos.Add(adjuntoDto);
            }
            correoDto.CorreosAdjuntos = adjuntos;

            if (correo.CorreoEml != null)
            {
                eml = new CorreoEmlDto()
                {
                    Id = correo.CorreoEml.Id,
                    Nombre = correo.CorreoEml.Nombre,
                    TamanoBytes = correo.CorreoEml.TamanoBytes,
                    ContenidoArchivo = correo.CorreoEml.ContenidoArchivo,
                    ContenidoArchivoB64 = Convert.ToBase64String(correo.CorreoEml.ContenidoArchivo)
                };
            }
            correoDto.CorreosEml = eml;

            //var texto = Encoding.UTF8.GetString(correo.CorreoEml.ContenidoArchivo);
            //Console.WriteLine(texto.Substring(0, 500));
            //File.WriteAllBytes(@"C:\Temp\correo_desde_bd.eml",correo.CorreoEml.ContenidoArchivo);


            return _apiResponse.CrearRespuesta<CorreoDto?>(true, "", correoDto);
        }

        private ECO_ColaSolicitud AgregarColaSolicitud(DatosCorreoRequest datoCorreoDto)
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

        private void CrearAdjuntos(int correoId, List<CorreoAdjuntoRequest> adjuntosB64, int usuarioCreadorId) 
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
