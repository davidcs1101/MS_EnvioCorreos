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

        public async Task<ECO_Configuracion?> ObtenerPorIdAsync(int id)
        {
            return await _context.ECO_Configuraciones.FindAsync(id);
        }

        public async Task<ECO_Configuracion?> ObtenerPorEmpresaIdYCodigoAsync(int empresaId, string codigo) 
        {
            return await _context.ECO_Configuraciones
                .FirstOrDefaultAsync(x => x.EmpresaId == empresaId && x.Codigo == codigo);
        }

        public async Task ModificarAsync(ECO_Configuracion correoConfiguracion)
        {
            _context.ECO_Configuraciones.Update(correoConfiguracion);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CrearAsync(ECO_Configuracion correoConfiguracion)
        {
            _context.ECO_Configuraciones.Add(correoConfiguracion);
            await _context.SaveChangesAsync();
            return correoConfiguracion.Id;
        }
    }
}
