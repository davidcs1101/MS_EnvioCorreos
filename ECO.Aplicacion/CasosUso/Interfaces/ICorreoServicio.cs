using ECO.Dtos;
namespace ECO.Aplicacion.CasosUso.Interfaces
{
    public interface ICorreoServicio
    {
        Task<ApiResponse<Guid?>> CrearAsync(CorreoCreacionRequest datosCorreoRequest);
        Task<ApiResponse<CorreoDto?>> ObtenerPorCodigoAsync(Guid codigo);
    }
}