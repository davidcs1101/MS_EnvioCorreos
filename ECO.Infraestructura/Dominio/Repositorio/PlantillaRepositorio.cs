using ECO.Dominio.Repositorio;
using ECO.DataAccess;
using ECO.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace ECO.Infraestructura.Dominio.Repositorio
{
    public class PlantillaRepositorio : IPlantillaRepositorio
    {
        private readonly AppDbContext _context;

        public PlantillaRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ECO_Plantilla?> ObtenerPorIdAsync(int id)
        {
            return await _context.ECO_Plantillas.FindAsync(id);
        }

        public async Task<ECO_Plantilla?> ObtenerPorEmpresaIdYCodigoAsync(int empresaId, string codigo) 
        {
            return await _context.ECO_Plantillas
                .FirstOrDefaultAsync(x => x.EmpresaId == empresaId && x.Codigo == codigo);
        }

        public async Task ModificarAsync(ECO_Plantilla plantilla)
        {
            _context.ECO_Plantillas.Update(plantilla);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CrearAsync(ECO_Plantilla plantilla)
        {
            _context.ECO_Plantillas.Add(plantilla);
            await _context.SaveChangesAsync();
            return plantilla.Id;
        }

        public IQueryable<ECO_Plantilla> ListarPorEmpresaId(int empresaId)
        {
            return _context.ECO_Plantillas.Where(x => x.EmpresaId == empresaId);
        }
    }
}
