using Microsoft.AspNetCore.Mvc;
using ECO.Dtos;
using ECO.Aplicacion.ServiciosExternos;
using ECO.Aplicacion.CasosUso.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ECO.Api.EnvioCorreos.Controllers
{
    [ApiController]
    [Route("api/correosConfiguracion")]
    public class CorreoConfiguracionController : Controller
    {
        private readonly ICorreoConfiguracionServicio _correoConfiguracionServicio;

        public CorreoConfiguracionController(ICorreoConfiguracionServicio correoConfiguracionServicio) 
        {
            _correoConfiguracionServicio = correoConfiguracionServicio;
        }

        [HttpPost("crear")]  
        [Authorize]
        public async Task<ActionResult<ApiResponse<int>>> Crear(CorreoConfiguracionCreacionRequest datosCorreoRequest) 
        {
            return await _correoConfiguracionServicio.CrearAsync(datosCorreoRequest);
        }

        [HttpGet("obtenerPorCodigo")]
        public async Task<ActionResult<ApiResponse<CorreoConfiguracionDto?>>> ObtenerPorCodigo(CorreoConfiguracionCreacionRequest datosCorreoConfiguracionRequest)
        {
            return await _correoConfiguracionServicio.ObtenerPorCodigoAsync(datosCorreoConfiguracionRequest.Codigo);
        }
    }
}
