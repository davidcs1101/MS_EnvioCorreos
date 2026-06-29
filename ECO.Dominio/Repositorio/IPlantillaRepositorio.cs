using ECO.Dominio.Entidades;

namespace ECO.Dominio.Repositorio
{
    public interface IPlantillaRepositorio
    {
        Task<ECO_Plantilla?> ObtenerPorIdAsync(int id);
        Task<ECO_Plantilla?> ObtenerPorEmpresaIdYCodigoAsync(int empresaId, string codigo);

        Task<int> CrearAsync(ECO_Plantilla plantilla);
        Task ModificarAsync(ECO_Plantilla plantilla);

        IQueryable<ECO_Plantilla> ListarPorEmpresaId(int empresaId);
    }
}
