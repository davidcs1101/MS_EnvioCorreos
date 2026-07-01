using Microsoft.AspNetCore.Mvc;
using ECO.Dtos;
using ECO.Aplicacion.ServiciosExternos;
using ECO.Aplicacion.CasosUso.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ECO.Api.EnvioCorreos.Controllers
{
    [ApiController]
    [Route("api/plantilla")]
    public class PlantillaController : Controller
    {
        private readonly IPlantillaServicio _plantillaServicio;

        public PlantillaController(IPlantillaServicio plantillaServicio)
        {
            _plantillaServicio = plantillaServicio;
        }

        [HttpPost("crear")]
        [Authorize]
        //[Authorize(Policy = "CrearPlantilla")]
        public async Task<ActionResult<ApiResponse<int>>> Crear(PlantillaCreacionRequest plantillaCreacionRequest)
        {
            return await _plantillaServicio.CrearAsync(plantillaCreacionRequest);
        }

        [HttpPut("modificar")]
        [Authorize]
        //[Authorize(Policy = "ModificarPlantilla")]
        public async Task<ApiResponse<string>> Modificar(PlantillaModificacionRequest plantillaModificacionRequest)
        {
            return await _plantillaServicio.ModificarAsync(plantillaModificacionRequest);
        }

        [HttpGet("obtenerPorCodigo")]
        [Authorize]
        //[Authorize(Policy = "ConsultarPlantilla")]
        public async Task<ActionResult<ApiResponse<PlantillaDto?>>> ObtenerPorCodigo(string codigo)
        {
            return await _plantillaServicio.ObtenerPorCodigoAsync(codigo);
        }

        [HttpGet("listarPorEmpresaId")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<List<PlantillaDto?>>>> ListarPorEmpresaId()
        {
            return await _plantillaServicio.ListarPorEmpresaIdAsync();
        }
    }
}
