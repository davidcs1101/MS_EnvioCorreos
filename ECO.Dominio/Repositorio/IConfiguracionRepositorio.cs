using ECO.Dominio.Entidades;

namespace ECO.Dominio.Repositorio
{
    public interface IConfiguracionRepositorio
    {
        Task<ECO_Configuracion?> ObtenerPorIdAsync(int id);
        Task<ECO_Configuracion?> ObtenerPorEmpresaIdYCodigoAsync(int empresaId, string codigo);

        Task<int> CrearAsync(ECO_Configuracion correoConfiguracion);
        Task ModificarAsync(ECO_Configuracion correoConfiguracion);    
    }
}
