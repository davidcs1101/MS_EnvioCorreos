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
using static Utilidades.Textos;


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
        private readonly IEntidadValidador<ECO_Plantilla> _plantillaValidadorServicio;
        private readonly IPlantillaRepositorio _plantillaRepositorio;
        private readonly IUsuarioContextoServicio _usuarioContextoServicio;

        private readonly IAppSettings _appSettings;

        public CorreoServicio(ICorreoRepositorio correoRepositorio, ICorreoDestinatarioRepositorio correoDestinatarioRepositorio, ICorreoAdjuntoRepositorio correoAdjuntoRepositorio, IColaSolicitudRepositorio colaSolicitudRepositorio, IMapperPerfiles mapper, IApiResponse apiResponse, IUnidadDeTrabajo unidadDeTrabajo, IProcesadorTransacciones procesadorTransacciones, ISerializadorJsonServicio serializadorJsonServicio, IAppSettings appSettings, IEntidadValidador<ECO_Correo> correoValidadorServicio, IPlantillaRepositorio plantillaRepositorio, IUsuarioContextoServicio usuarioContextoServicio, IEntidadValidador<ECO_Plantilla> plantillaValidadorServicio)
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
            _plantillaRepositorio = plantillaRepositorio;
            _usuarioContextoServicio = usuarioContextoServicio;
            _plantillaValidadorServicio = plantillaValidadorServicio;
        }

        public async Task<ApiResponse<Guid?>> CrearAsync(CorreoCreacionRequest datosCorreoRequest)
        {
            var id = 0;
            var cola = new ECO_ColaSolicitud();
            var trazabilidadCorreo = _appSettings.ObtenerTrazabilidadCorreoSettings();
            Guid? codigo = null;
            string? codigoConfiguracionEnvio = null;
            int? empresaId = null;
            int? plantillaId = null;
            await _procesadorTransacciones.EjecutarEnTransaccionAsync(async () =>
            {
                if (datosCorreoRequest is CorreoEmpresaCreacionRequest datosCorreoEmpresaRequest)
                {
                    empresaId = _usuarioContextoServicio.ValidarEmpresaIdToken(datosCorreoEmpresaRequest.EmpresaId);
                    var plantilla = await ObtenerPlantillaPorEmpresaIdYCodigoAsync(empresaId, datosCorreoEmpresaRequest.Plantilla);
                    if (plantilla is not null)
                    {
                        plantillaId = plantilla.Id;
                        datosCorreoRequest.Asunto = ProcesarVariables(plantilla.Asunto, datosCorreoEmpresaRequest.Plantilla!.Variables);
                        datosCorreoRequest.Cuerpo = ProcesarVariables(plantilla.Html, datosCorreoEmpresaRequest.Plantilla!.Variables);
                        datosCorreoRequest.EsCuerpoHtml = true;
                    }
                    codigoConfiguracionEnvio = datosCorreoEmpresaRequest.CodigoConfiguracionEnvio;
                }


                //Creamos ECO_Correo
                var correo = _mapper.DatoCorreoRequestACorreo(datosCorreoRequest);
                correo.Estado = EstadoCorreo.Pendiente;
                correo.EmpresaId = empresaId;
                correo.PlantillaId = plantillaId;

                _correoRepositorio.MarcarCrear(correo);
                await _unidadDeTrabajo.GuardarCambiosAsync();
                id = correo.Id;
                var usuarioId = correo.UsuarioCreadorId;
                codigo = correo.Codigo;

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

                var guardarEmlCorreo =
                    trazabilidadCorreo.GuardarEmlCorreo ||
                    datosCorreoRequest.Acciones.GuardarEmlCorreo;


                var datosCorreo = _mapper.DatoCorreoRequestADatoCorreoDto(datosCorreoRequest);
                datosCorreo.CorreoId = id;
                datosCorreo.CodigoConfiguracionEnvio = codigoConfiguracionEnvio;
                datosCorreo.EmpresaId = correo.EmpresaId;
                datosCorreo.GuardarEmlCorreo = guardarEmlCorreo;
                cola = this.AgregarColaSolicitud(datosCorreo);

                await _unidadDeTrabajo.GuardarCambiosAsync();

            });

            return _apiResponse.CrearRespuesta(true, Textos.Generales.MENSAJE_REGISTRO_CREADO, codigo);
        }

        public async Task<ApiResponse<CorreoDto?>> ObtenerCorreoPorCodigoAsync(Guid codigo) 
        {
            var correo = await _correoRepositorio.ObtenerPorCodigoAsync(codigo);
            _correoValidadorServicio.ValidarDatoNoEncontrado(correo, Textos.Correos.MENSAJE_CORREO_NO_EXISTE_ID);

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
                CodigoPlantilla = correo.PlantillaId.HasValue ? correo.Plantilla!.Codigo : null
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
                };
                adjuntos.Add(adjuntoDto);
            }
            correoDto.CorreosAdjuntos = adjuntos;

            return _apiResponse.CrearRespuesta<CorreoDto?>(true, "", correoDto);
        }

        public async Task<ApiResponse<CorreoEmlDto?>> ObtenerEmlPorCodigoAsync(Guid codigo)
        {
            var correo = await _correoRepositorio.ObtenerPorCodigoAsync(codigo);
            _correoValidadorServicio.ValidarDatoNoEncontrado(correo, Textos.Correos.MENSAJE_CORREO_NO_EXISTE_ID);

            CorreoEmlDto? eml = null;
            if (correo.CorreoEml != null)
            {
                eml = new CorreoEmlDto()
                {
                    Id = correo.CorreoEml.Id,
                    Nombre = correo.CorreoEml.Nombre,
                    TamanoBytes = correo.CorreoEml.TamanoBytes,
                    ContenidoArchivo = correo.CorreoEml.ContenidoArchivo,
                    FechaCreado = correo.CorreoEml.FechaCreado,
                    UsuarioCreadorId = correo.CorreoEml.UsuarioCreadorId
                };
            }
            return _apiResponse.CrearRespuesta<CorreoEmlDto?>(true, "", eml);
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

        private async Task<ECO_Plantilla?> ObtenerPlantillaPorEmpresaIdYCodigoAsync(int? empresaId, PlantillaRequest? plantillaRequest)
        {
            if (empresaId is null || string.IsNullOrWhiteSpace(plantillaRequest?.Codigo))
                return null;

            var plantilla = await _plantillaRepositorio.ObtenerPorEmpresaIdYCodigoAsync(empresaId.Value, plantillaRequest.Codigo);
            _plantillaValidadorServicio.ValidarDatoNoEncontrado(plantilla, Textos.Plantillas.MENSAJE_PLANTILLAS_NO_EXISTE_CODIGO);

            return plantilla;
        }
        private string ProcesarVariables(string texto, Dictionary<string, string>? variables)
        {
            if (variables == null)
                return texto;

            foreach (var variable in variables)
            {
                texto = texto.Replace(
                    $"{{{{{variable.Key}}}}}",
                    variable.Value?.ToString() ?? "");
            }
            return texto;
        }
    }
}
