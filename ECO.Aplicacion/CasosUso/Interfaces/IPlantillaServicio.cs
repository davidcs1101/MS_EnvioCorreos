using ECO.Dtos;
namespace ECO.Aplicacion.CasosUso.Interfaces
{
    public interface IPlantillaServicio
    {
        Task<ApiResponse<int>> CrearAsync(PlantillaCreacionRequest plantillaCreacionRequest);
        Task<ApiResponse<PlantillaDto?>> ObtenerPorCodigoAsync(string codigo);
        Task<ApiResponse<List<PlantillaDto?>>> ListarPorEmpresaIdAsync();
        Task<ApiResponse<string>> ModificarAsync(PlantillaModificacionRequest plantillaModificacionRequest);
    }
}