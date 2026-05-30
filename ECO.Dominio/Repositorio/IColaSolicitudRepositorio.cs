using ECO.Dominio.Entidades;

namespace ECO.Dominio.Repositorio
{
    public interface IColaSolicitudRepositorio
    {
        void MarcarCrear(ECO_ColaSolicitud colaSolicitud);
        void MarcarModificar(ECO_ColaSolicitud colaSolicitud);
        Task<ECO_ColaSolicitud?> ObtenerPorIdAsync(int id);
        IQueryable<ECO_ColaSolicitud> Listar();
    }
}
