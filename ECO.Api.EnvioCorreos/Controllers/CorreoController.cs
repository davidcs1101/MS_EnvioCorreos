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
        public async Task<ActionResult<ApiResponse<int>>> EnviarCorreo(DatosCorreoRequest datosCorreoRequest) 
        {
            return await _correoServicio.CrearAsync(datosCorreoRequest);
        }

        [HttpPost("enviarCorreoEmpresa")]
        [Authorize(Policy = "EnviarCorreoEmpresa")]
        public async Task<ActionResult<ApiResponse<int>>> EnviarCorreoEmpresa(DatosCorreoEmpresaRequest datosCorreoEmpresaRequest)
        {
            return await _correoServicio.CrearAsync(datosCorreoEmpresaRequest);
        }


        //AUN FALTA AGREGARLO AL GATEWAY
        [HttpGet("obtenerPorId")]
        [Authorize(Policy = "ConsultarCorreoEmpresa")]
        public async Task<ActionResult<ApiResponse<int?>>> ObtenerPorId(int id)
        {
            return new ApiResponse<int?> { Data = id };
            //return await _listaServicio.ObtenerPorIdAsync(id);
        }
    }
}
