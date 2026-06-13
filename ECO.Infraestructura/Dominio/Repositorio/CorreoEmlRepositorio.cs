using ECO.Dominio.Repositorio;
using ECO.DataAccess;
using ECO.Dominio.Entidades;

namespace ECO.Infraestructura.Dominio.Repositorio
{
    public class CorreoEmlRepositorio : ICorreoEmlRepositorio
    {
        private readonly AppDbContext _context;

        public CorreoEmlRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> CrearAsync(ECO_CorreoEml correoEml)
        {
            _context.ECO_CorreosEml.Add(correoEml);
            await _context.SaveChangesAsync();
            return correoEml.Id;
        }
    }
}
