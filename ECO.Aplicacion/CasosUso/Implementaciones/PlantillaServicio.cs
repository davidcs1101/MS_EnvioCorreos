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
    public class PlantillaServicio : IPlantillaServicio
    {
        private readonly IPlantillaRepositorio _plantillaRepositorio;

        private readonly IMapperPerfiles _mapper;
        private readonly IApiResponse _apiResponse;

        private readonly ISerializadorJsonServicio _serializadorJsonServicio;
        private readonly IEntidadValidador<ECO_Plantilla> _plantillaValidadorServicio;
        private readonly IUsuarioContextoServicio _usuarioContextoServicio;

        private readonly IAppSettings _appSettings;


        public PlantillaServicio(IPlantillaRepositorio plantillaRepositorio, IMapperPerfiles mapper, 
            IApiResponse apiResponse, ISerializadorJsonServicio serializadorJsonServicio, 
            IAppSettings appSettings, IEntidadValidador<ECO_Plantilla> plantillaValidadorServicio, 
            IUsuarioContextoServicio usuarioContextoServicio)
        {
            _plantillaRepositorio = plantillaRepositorio;
            _mapper = mapper;
            _apiResponse = apiResponse;
            _serializadorJsonServicio = serializadorJsonServicio;
            _appSettings = appSettings;
            _plantillaValidadorServicio = plantillaValidadorServicio;
            _usuarioContextoServicio = usuarioContextoServicio;
        }

        public async Task<ApiResponse<int>> CrearAsync(PlantillaCreacionRequest plantillaCreacionRequest)
        {
            var empresaId = _usuarioContextoServicio.ObtenerEmpresaIdToken();
            var usuarioId = _usuarioContextoServicio.ObtenerUsuarioIdToken();

            var plantillaExiste = await _plantillaRepositorio.ObtenerPorEmpresaIdYCodigoAsync(empresaId, plantillaCreacionRequest.Codigo);
            _plantillaValidadorServicio.ValidarDatoYaExiste(plantillaExiste, Textos.Plantillas.MENSAJE_PLANTILLA_CODIGO_EXISTE);

            var plantilla = _mapper.PlantillaCreacionRequestAPlantilla(plantillaCreacionRequest);
            plantilla.EmpresaId = empresaId;
            plantilla.UsuarioCreadorId = usuarioId;

            var id = await _plantillaRepositorio.CrearAsync(plantilla);

            return _apiResponse.CrearRespuesta(true, Textos.Generales.MENSAJE_REGISTRO_CREADO, id);
        }

        public async Task<ApiResponse<string>> ModificarAsync(PlantillaModificacionRequest plantillaModificacionRequest)
        {
            var plantillaExiste = await _plantillaRepositorio.ObtenerPorIdAsync(plantillaModificacionRequest.Id);
            _plantillaValidadorServicio.ValidarDatoNoEncontrado(plantillaExiste, Textos.Plantillas.MENSAJE_PLANTILLA_NO_EXISTE_ID);

            _mapper.PlantillaModificacionRequestAPlantilla(plantillaModificacionRequest, plantillaExiste);
            plantillaExiste!.FechaModificado = DateTime.UtcNow;
            plantillaExiste!.UsuarioModificadorId = _usuarioContextoServicio.ObtenerUsuarioIdToken();

            await _plantillaRepositorio.ModificarAsync(plantillaExiste);

            return _apiResponse.CrearRespuesta(true, Textos.Generales.MENSAJE_REGISTRO_ACTUALIZADO, "");
        }


        public async Task<ApiResponse<PlantillaDto?>> ObtenerPorCodigoAsync(string codigo) 
        {
            var empresaId = _usuarioContextoServicio.ObtenerEmpresaIdToken();
            var plantilla = await _plantillaRepositorio.ObtenerPorEmpresaIdYCodigoAsync(empresaId, codigo);
            _plantillaValidadorServicio.ValidarDatoNoEncontrado(plantilla, Textos.Plantillas.MENSAJE_PLANTILLA_NO_EXISTE_CODIGO);

            var plantillaDto = _mapper.PlantillaAPlantillaDto(plantilla);
            return _apiResponse.CrearRespuesta<PlantillaDto?>(true, "", plantillaDto);
        }

        public async Task<ApiResponse<List<PlantillaDto?>>> ListarPorEmpresaIdAsync()
        {
            var empresaId = _usuarioContextoServicio.ObtenerEmpresaIdToken();
            var plantillas = _plantillaRepositorio.ListarPorEmpresaId(empresaId).ToList();
            var plantillasDto = plantillas.Select(p => _mapper.PlantillaAPlantillaDto(p)).ToList();
            return _apiResponse.CrearRespuesta<List<PlantillaDto?>>(true, "", plantillasDto);
        }

    }
}
