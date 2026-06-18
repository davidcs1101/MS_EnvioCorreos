using ECO.Dominio.Repositorio;
using ECO.DataAccess;
using ECO.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace ECO.Infraestructura.Dominio.Repositorio
{
    public class CorreoConfiguracionRepositorio : ICorreoConfiguracionRepositorio
    {
        private readonly AppDbContext _context;

        public CorreoConfiguracionRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ECO_CorreoConfiguracion?> ObtenerPorIdAsync(int id)
        {
            return await _context.ECO_CorreosConfiguraciones.FindAsync(id);
        }

        public async Task<ECO_CorreoConfiguracion?> ObtenerPorEmpresaIdYCodigoAsync(int empresaId, string codigo) 
        {
            return await _context.ECO_CorreosConfiguraciones
                .FirstOrDefaultAsync(x => x.EmpresaId == empresaId && x.Codigo == codigo);
        }

        public async Task ModificarAsync(ECO_CorreoConfiguracion correoConfiguracion)
        {
            _context.ECO_CorreosConfiguraciones.Update(correoConfiguracion);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CrearAsync(ECO_CorreoConfiguracion correoConfiguracion)
        {
            _context.ECO_CorreosConfiguraciones.Add(correoConfiguracion);
            await _context.SaveChangesAsync();
            return correoConfiguracion.Id;
        }
    }
}
