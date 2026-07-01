using ECO.Dtos;
namespace ECO.Aplicacion.CasosUso.Interfaces
{
    public interface IConfiguracionServicio
    {
        Task<ApiResponse<int>> CrearAsync(ConfiguracionCreacionRequest CorreoConfiguracionCreacionRequest);
        Task<ApiResponse<ConfiguracionDto?>> ObtenerPorCodigoAsync(string codigo);
        Task<ApiResponse<List<ConfiguracionDto?>>> ListarPorEmpresaIdAsync();
        Task<ApiResponse<string>> ModificarAsync(ConfiguracionModificacionRequest configuracionModificacionRequest);
    }
}