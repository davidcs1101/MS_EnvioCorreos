using Microsoft.AspNetCore.Http;
using ECO.Aplicacion.ServiciosExternos;

namespace ECO.Infraestructura.Aplicacion.ServiciosExternos
{
    public class UsuarioContextoServicio : IUsuarioContextoServicio
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioContextoServicio(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int ObtenerUsuarioIdToken()
        {
            // Obtener el 'UsuarioId' desde el token JWT en el contexto HTTP
            var usuarioIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("UsuarioId")?.Value;

            if (string.IsNullOrEmpty(usuarioIdClaim))
                throw new UnauthorizedAccessException("No se encontró el 'UsuarioId' en el token JWT.");

            return Convert.ToInt32(usuarioIdClaim);
        }

        public int ObtenerEmpresaIdToken()
        {
            // Obtener el 'EmpresaId' desde el token JWT en el contexto HTTP
            var empresaIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("EmpresaId")?.Value;

            if (string.IsNullOrEmpty(empresaIdClaim))
                throw new UnauthorizedAccessException("No se encontró la 'EmpresaId' en el token JWT.");

            return Convert.ToInt32(empresaIdClaim);
        }
    }
}
