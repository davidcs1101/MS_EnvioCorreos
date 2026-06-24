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
    public class CorreoConfiguracionServicio : ICorreoConfiguracionServicio
    {
        private readonly ICorreoConfiguracionRepositorio _correoConfiguracionRepositorio;

        private readonly IMapperPerfiles _mapper;
        private readonly IApiResponse _apiResponse;

        private readonly ISerializadorJsonServicio _serializadorJsonServicio;
        private readonly IEntidadValidador<ECO_Configuracion> _correoConfiguracionValidadorServicio;
        private readonly IUsuarioContextoServicio _usuarioContextoServicio;

        private readonly IAppSettings _appSettings;


        public CorreoConfiguracionServicio(ICorreoConfiguracionRepositorio correoConfiguracionRepositorio, IMapperPerfiles mapper, IApiResponse apiResponse, ISerializadorJsonServicio serializadorJsonServicio, IAppSettings appSettings, IEntidadValidador<ECO_Configuracion> correoConfiguracionValidadorServicio, IUsuarioContextoServicio usuarioContextoServicio)
        {
            _correoConfiguracionRepositorio = correoConfiguracionRepositorio;
            _mapper = mapper;
            _apiResponse = apiResponse;
            _serializadorJsonServicio = serializadorJsonServicio;
            _appSettings = appSettings;
            _correoConfiguracionValidadorServicio = correoConfiguracionValidadorServicio;
            _usuarioContextoServicio = usuarioContextoServicio;
        }

        public async Task<ApiResponse<int>> CrearAsync(CorreoConfiguracionCreacionRequest correoConfiguracionCreacionRequest)
        {
            var empresaId = _usuarioContextoServicio.ObtenerEmpresaIdToken();
            var usuarioId = _usuarioContextoServicio.ObtenerUsuarioIdToken();

            var correoConfiguracionExiste = await _correoConfiguracionRepositorio.ObtenerPorEmpresaIdYCodigoAsync(empresaId, correoConfiguracionCreacionRequest.Codigo);
            _correoConfiguracionValidadorServicio.ValidarDatoYaExiste(correoConfiguracionExiste, Textos.CorreosConfiguraciones.MENSAJE_CORREOCONFIGURACION_CODIGO_EXISTE);

            var correoConfiguracion = _mapper.CorreoConfiguracionCreacionRequestACorreoConfiguracion(correoConfiguracionCreacionRequest);
            correoConfiguracion.EmpresaId = empresaId;
            correoConfiguracion.UsuarioCreadorId = usuarioId;

            var id = await _correoConfiguracionRepositorio.CrearAsync(correoConfiguracion);

            return _apiResponse.CrearRespuesta(true, Textos.Generales.MENSAJE_REGISTRO_CREADO, id);
        }

        public async Task<ApiResponse<CorreoConfiguracionDto?>> ObtenerPorCodigoAsync(string codigo) 
        {
            var empresaId = _usuarioContextoServicio.ObtenerEmpresaIdToken();
            var correoConfiguracion = await _correoConfiguracionRepositorio.ObtenerPorEmpresaIdYCodigoAsync(empresaId, codigo);
            _correoConfiguracionValidadorServicio.ValidarDatoNoEncontrado(correoConfiguracion, Textos.CorreosConfiguraciones.MENSAJE_CORREOCONFIGURACION_NO_EXISTE_CODIGO);

            var correoConfiguracionDto = _mapper.CorreoConfiguracionACorreoConfiguracionDto(correoConfiguracion);
            return _apiResponse.CrearRespuesta<CorreoConfiguracionDto?>(true, "", correoConfiguracionDto);
        }


    }
}
