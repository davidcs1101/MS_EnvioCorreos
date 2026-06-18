using ECO.Dtos;
namespace ECO.Aplicacion.CasosUso.Interfaces
{
    public interface ICorreoConfiguracionServicio
    {
        Task<ApiResponse<int>> CrearAsync(CorreoConfiguracionCreacionRequest CorreoConfiguracionCreacionRequest);
        Task<ApiResponse<CorreoConfiguracionDto?>> ObtenerPorCodigoAsync(string codigo);
    }
}