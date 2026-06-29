using ECO.Dtos;
namespace ECO.Aplicacion.CasosUso.Interfaces
{
    public interface ICorreoServicio
    {
        Task<ApiResponse<Guid?>> CrearAsync(CorreoCreacionRequest datosCorreoRequest);
        Task<ApiResponse<CorreoDto?>> ObtenerCorreoPorCodigoAsync(Guid codigo);
        Task<ApiResponse<CorreoEmlDto?>> ObtenerEmlPorCodigoAsync(Guid codigo);
    }
}