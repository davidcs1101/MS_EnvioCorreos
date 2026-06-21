using ECO.Dominio.Repositorio;
using ECO.DataAccess;
using ECO.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

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

        public async Task<ECO_Correo?> ObtenerPorCodigoAsync(Guid codigo)
        {
            return await _context.ECO_Correos
                .Include(x => x.CorreosAdjuntos)
                .Include(x => x.CorreosDestinatarios)
                .Include(x => x.CorreoEml)
                .FirstOrDefaultAsync(x => x.Codigo == codigo);
        }

        public async Task ModificarAsync(ECO_Correo correo)
        {
            _context.ECO_Correos.Update(correo);
            await _context.SaveChangesAsync();
        }
    }
}
