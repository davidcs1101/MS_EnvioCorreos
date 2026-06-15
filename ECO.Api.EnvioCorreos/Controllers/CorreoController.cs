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
        public async Task<ActionResult<ApiResponse<int>>> EnviarCorreo(CorreoCreacionRequest datosCorreoRequest) 
        {
            return await _correoServicio.CrearAsync(datosCorreoRequest);
        }

        [HttpPost("enviarCorreoEmpresa")]
        [Authorize(Policy = "EnviarCorreoEmpresa")]
        public async Task<ActionResult<ApiResponse<int>>> EnviarCorreoEmpresa(CorreoEmpresaCreacionRequest datosCorreoEmpresaRequest)
        {
            return await _correoServicio.CrearAsync(datosCorreoEmpresaRequest);
        }


        //AUN FALTA AGREGARLO AL GATEWAY
        [HttpGet("obtenerPorId")]
        //[Authorize(Policy = "ConsultarCorreoEmpresa")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<CorreoDto?>>> ObtenerPorId(int id)
        {
            return await _correoServicio.ObtenerPorIdYEmpresaIdAsync(id);
        }
    }
}
