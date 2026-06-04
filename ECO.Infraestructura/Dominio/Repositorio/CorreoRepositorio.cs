using ECO.Dominio.Repositorio;
using ECO.DataAccess;
using ECO.Dominio.Entidades;

namespace ECO.Infraestructura.Dominio.Repositorio
{
    public class CorreoRepositorio : ICorreoRepositorio
    {
        private readonly AppDbContext _context;

        public CorreoRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public void MarcarCrear(ECO_Correo correo)
        {
            _context.ECO_Correos.Add(correo);
        }

        public void MarcarModificar(ECO_Correo correo)
        {

            _context.ECO_Correos.Update(correo);
        }

        public async Task<ECO_Correo?> ObtenerPorIdAsync(int id) 
        {
            return await _context.ECO_Correos.FindAsync(id);
        }
    }
}
