using ECO.Dominio.Entidades;

namespace ECO.Dominio.Repositorio
{
    public interface ICorreoConfiguracionRepositorio
    {
        Task<ECO_CorreoConfiguracion?> ObtenerPorIdAsync(int id);
        Task<ECO_CorreoConfiguracion?> ObtenerPorEmpresaIdYCodigoAsync(int empresaId, string codigo);

        Task<int> CrearAsync(ECO_CorreoConfiguracion correoConfiguracion);
        Task ModificarAsync(ECO_CorreoConfiguracion correoConfiguracion);    
    }
}
