using ECO.Dtos;
namespace ECO.Aplicacion.CasosUso.Interfaces
{
    public interface ICorreoServicio
    {
        Task<ApiResponse<int>> CrearAsync(DatosCorreoRequest datosCorreoRequest);
        //Task<ApiResponse<CorreoDto?>> ObtenerPorIdAsync(int id);
    }
}
