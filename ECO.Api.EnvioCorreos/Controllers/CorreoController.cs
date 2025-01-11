using Microsoft.AspNetCore.Mvc;
using ECO.Dtos;
using ECO.Servicio;
using ECO.Servicio.Interfaces;
using Microsoft.Extensions.Configuration;
using Utilidades;
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
        public async Task<ActionResult<ApiResponse<string>>> EnviarCorreo(DatoCorreoDto datosCorreoDto) 
        {
            return await _correoServicio.EnviarCorreoAsync(datosCorreoDto);
        }
    }
}
