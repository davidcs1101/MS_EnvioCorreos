﻿using Microsoft.AspNetCore.Mvc;
using ECO.Dtos;
using ECO.Aplicacion.CasosUso.Interfaces;

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
