using Microsoft.AspNetCore.Mvc;
using ECO.Dtos;
using ECO.Aplicacion.ServiciosExternos;
using ECO.Aplicacion.CasosUso.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ECO.Api.EnvioCorreos.Controllers
{
    [ApiController]
    [Route("api/correos")]
    public class CorreoController : Controller
    {
        private readonly ICorreoServicio _correoServicio;

        public CorreoController(ICorreoServicio correoServicio) 
        {
            _correoServicio = correoServicio;
        }

        [HttpPost("enviarCorreo")]  
        [Authorize]
        public async Task<ActionResult<ApiResponse<Guid?>>> EnviarCorreo(CorreoCreacionRequest datosCorreoRequest) 
        {
            return await _correoServicio.CrearAsync(datosCorreoRequest);
        }

        [HttpPost("enviarCorreoEmpresa")]
        [Authorize]
        //[Authorize(Policy = "EnviarCorreoEmpresa")]
        public async Task<ActionResult<ApiResponse<Guid?>>> EnviarCorreoEmpresa(CorreoEmpresaCreacionRequest datosCorreoEmpresaRequest)
        {
            return await _correoServicio.CrearAsync(datosCorreoEmpresaRequest);
        }

        [HttpGet("obtenerCorreoPorCodigo")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<CorreoDto?>>> ObtenerCorreoPorCodigo(Guid codigo)
        {
            return await _correoServicio.ObtenerCorreoPorCodigoAsync(codigo);
        }

        [HttpGet("obtenerEmlPorCodigo")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<CorreoEmlDto?>>> ObtenerEmlPorCodigo(Guid codigo)
        {
            return await _correoServicio.ObtenerEmlPorCodigoAsync(codigo);
        }
    }
}
