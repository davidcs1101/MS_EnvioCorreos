using ECO.Dtos;
namespace ECO.Aplicacion.CasosUso.Interfaces
{
    public interface ICorreoServicio
    {
        Task<ApiResponse<int>> CrearAsync(CorreoCreacionRequest datosCorreoRequest);
        Task<ApiResponse<CorreoDto?>> ObtenerPorIdYEmpresaIdAsync(int id);
    }
}