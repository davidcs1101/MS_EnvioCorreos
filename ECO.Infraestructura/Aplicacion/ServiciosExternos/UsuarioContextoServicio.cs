using Microsoft.AspNetCore.Http;
using ECO.Aplicacion.ServiciosExternos;
using System.Diagnostics.CodeAnalysis;

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
                throw new UnauthorizedAccessException("No se encontró el 'EmpresaId' en el token JWT.");
            return Convert.ToInt32(empresaIdClaim);
        }

        /// <summary>
        /// Obtiene el 'EmpresaId' desde el token JWT y lo compara con el 'EmpresaId' proporcionado en el request.
        /// Si el 'EmpresaId' en el token es 1                                                -> entonces el usuario administrador es quien está haciendo el proceso.
        /// Si el 'EmpresaId' en el token es igual al 'EmpresaId' proporcionado en el request -> entonces el usuario pertenece a la empresa y puede hacer el proceso.
        /// </summary>
        /// <param name="empresaIdBody"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public int ValidarEmpresaIdToken(int empresaIdBody)
        {
            var empresaIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("EmpresaId")?.Value;

            if (string.IsNullOrWhiteSpace(empresaIdClaim))
                throw new UnauthorizedAccessException(
                    "No se encontró la 'EmpresaId' en el token JWT.");

            var empresaIdToken = Convert.ToInt32(empresaIdClaim);

            // EmpresaId del Usuario administrador/integración
            if (empresaIdToken == 1)
                return empresaIdBody;

            // EmpresaId del Usuario de empresa
            if (empresaIdToken == empresaIdBody)
                return empresaIdBody;

            throw new UnauthorizedAccessException(
                "El 'EmpresaId' en el token JWT no coincide con el 'EmpresaId' proporcionado en el request.");
        }
    }
}
