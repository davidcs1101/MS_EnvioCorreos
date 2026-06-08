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
        public async Task<ActionResult<ApiResponse<int>>> EnviarCorreo(DatosCorreoRequest datosCorreoDto) 
        {
            return await _correoServicio.CrearAsync(datosCorreoDto);
        }
    }
}
