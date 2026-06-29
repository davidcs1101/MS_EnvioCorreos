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
    public class ConfiguracionServicio : ICorreoConfiguracionServicio
    {
        private readonly IConfiguracionRepositorio _configuracionRepositorio;

        private readonly IMapperPerfiles _mapper;
        private readonly IApiResponse _apiResponse;

        private readonly ISerializadorJsonServicio _serializadorJsonServicio;
        private readonly IEntidadValidador<ECO_Configuracion> _configuracionValidadorServicio;
        private readonly IUsuarioContextoServicio _usuarioContextoServicio;

        private readonly IAppSettings _appSettings;


        public ConfiguracionServicio(IConfiguracionRepositorio configuracionRepositorio, IMapperPerfiles mapper, IApiResponse apiResponse, ISerializadorJsonServicio serializadorJsonServicio, IAppSettings appSettings, IEntidadValidador<ECO_Configuracion> configuracionValidadorServicio, IUsuarioContextoServicio usuarioContextoServicio)
        {
            _configuracionRepositorio = configuracionRepositorio;
            _mapper = mapper;
            _apiResponse = apiResponse;
            _serializadorJsonServicio = serializadorJsonServicio;
            _appSettings = appSettings;
            _configuracionValidadorServicio = configuracionValidadorServicio;
            _usuarioContextoServicio = usuarioContextoServicio;
        }

        public async Task<ApiResponse<int>> CrearAsync(ConfiguracionCreacionRequest correoConfiguracionCreacionRequest)
        {
            var empresaId = _usuarioContextoServicio.ObtenerEmpresaIdToken();
            var usuarioId = _usuarioContextoServicio.ObtenerUsuarioIdToken();

            var correoConfiguracionExiste = await _configuracionRepositorio.ObtenerPorEmpresaIdYCodigoAsync(empresaId, correoConfiguracionCreacionRequest.Codigo);
            _configuracionValidadorServicio.ValidarDatoYaExiste(correoConfiguracionExiste, Textos.Configuraciones.MENSAJE_CONFIGURACION_CODIGO_EXISTE);

            var correoConfiguracion = _mapper.ConfiguracionCreacionRequestACorreoConfiguracion(correoConfiguracionCreacionRequest);
            correoConfiguracion.EmpresaId = empresaId;
            correoConfiguracion.UsuarioCreadorId = usuarioId;

            var id = await _configuracionRepositorio.CrearAsync(correoConfiguracion);

            return _apiResponse.CrearRespuesta(true, Textos.Generales.MENSAJE_REGISTRO_CREADO, id);
        }

        public async Task<ApiResponse<string>> ModificarAsync(ConfiguracionModificacionRequest configuracionModificacionRequest)
        {
            var configuracionExiste = await _configuracionRepositorio.ObtenerPorIdAsync(configuracionModificacionRequest.Id);
            _configuracionValidadorServicio.ValidarDatoNoEncontrado(configuracionExiste, Textos.Configuraciones.MENSAJE_CONFIGURACION_NO_EXISTE_ID);

            _mapper.ConfiguracionModificacionRequestACorreoConfiguracion(configuracionModificacionRequest, configuracionExiste);
            configuracionExiste!.FechaModificado = DateTime.UtcNow;
            configuracionExiste!.UsuarioModificadorId = _usuarioContextoServicio.ObtenerUsuarioIdToken();

            await _configuracionRepositorio.ModificarAsync(configuracionExiste);

            return _apiResponse.CrearRespuesta(true, Textos.Generales.MENSAJE_REGISTRO_ACTUALIZADO, "");
        }


        public async Task<ApiResponse<ConfiguracionDto?>> ObtenerPorCodigoAsync(string codigo) 
        {
            var empresaId = _usuarioContextoServicio.ObtenerEmpresaIdToken();
            var correoConfiguracion = await _configuracionRepositorio.ObtenerPorEmpresaIdYCodigoAsync(empresaId, codigo);
            _configuracionValidadorServicio.ValidarDatoNoEncontrado(correoConfiguracion, Textos.Configuraciones.MENSAJE_CONFIGURACION_NO_EXISTE_CODIGO);

            var correoConfiguracionDto = _mapper.ConfiguracionACorreoConfiguracionDto(correoConfiguracion);
            return _apiResponse.CrearRespuesta<ConfiguracionDto?>(true, "", correoConfiguracionDto);
        }

        public async Task<ApiResponse<List<ConfiguracionDto?>>> ListarPorEmpresaIdAsync()
        {
            var empresaId = _usuarioContextoServicio.ObtenerEmpresaIdToken();
            var configuraciones = _configuracionRepositorio.ListarPorEmpresaId(empresaId).ToList();
            var correoConfiguracionesDto = configuraciones.Select(c => _mapper.ConfiguracionACorreoConfiguracionDto(c)).ToList();
            return _apiResponse.CrearRespuesta<List<ConfiguracionDto?>>(true, "", correoConfiguracionesDto);
        }

    }
}
