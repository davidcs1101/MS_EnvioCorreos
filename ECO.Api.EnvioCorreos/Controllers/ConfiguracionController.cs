using Microsoft.AspNetCore.Mvc;
using ECO.Dtos;
using ECO.Aplicacion.ServiciosExternos;
using ECO.Aplicacion.CasosUso.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ECO.Api.EnvioCorreos.Controllers
{
    [ApiController]
    [Route("api/configuracion")]
    public class ConfiguracionController : Controller
    {
        private readonly ICorreoConfiguracionServicio _correoConfiguracionServicio;

        public ConfiguracionController(ICorreoConfiguracionServicio correoConfiguracionServicio)
        {
            _correoConfiguracionServicio = correoConfiguracionServicio;
        }

        [HttpPost("crear")]
        [Authorize]
        //[Authorize(Policy = "CrearCorreoConfiguracion")]
        public async Task<ActionResult<ApiResponse<int>>> Crear(ConfiguracionCreacionRequest datosCorreoRequest)
        {
            return await _correoConfiguracionServicio.CrearAsync(datosCorreoRequest);
        }

        [HttpPut("modificar")]
        [Authorize]
        //[Authorize(Policy = "CrearCorreoConfiguracion")]
        public async Task<ApiResponse<string>> Modificar(ConfiguracionModificacionRequest datosCorreoRequest)
        {
            return await _correoConfiguracionServicio.ModificarAsync(datosCorreoRequest);
        }

        [HttpGet("obtenerPorCodigo")]
        [Authorize]
        //[Authorize(Policy = "ConsultarCorreoConfiguracion")]
        public async Task<ActionResult<ApiResponse<ConfiguracionDto?>>> ObtenerPorCodigo(string codigo)
        {
            return await _correoConfiguracionServicio.ObtenerPorCodigoAsync(codigo);
        }

        [HttpGet("listarPorEmpresaId")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<List<ConfiguracionDto?>>>> ListarPorEmpresaId()
        {
            return await _correoConfiguracionServicio.ListarPorEmpresaIdAsync();
        }
    }
}
