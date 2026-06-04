using ECO.Dominio.Repositorio;
using ECO.DataAccess;
using ECO.Dominio.Entidades;

namespace ECO.Infraestructura.Dominio.Repositorio
{
    public class ColaSolicitudRepositorio : IColaSolicitudRepositorio
    {
        private readonly AppDbContext _context;

        public ColaSolicitudRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public void MarcarCrear(ECO_ColaSolicitud colaSolicitud)
        {
            _context.ECO_ColaSolicitudes.Add(colaSolicitud);
        }

        public void MarcarModificar(ECO_ColaSolicitud colaSolicitud)
        {

            _context.ECO_ColaSolicitudes.Update(colaSolicitud);
        }

        public async Task<ECO_ColaSolicitud?> ObtenerPorIdAsync(int id) 
        {
            return await _context.ECO_ColaSolicitudes.FindAsync(id);
        }

        public IQueryable<ECO_ColaSolicitud> Listar()
        {
            return _context.ECO_ColaSolicitudes;
        }
    }
}
