using ECO.DataAccess;
using ECO.Dominio.Repositorio.UnidadTrabajo;

namespace ECO.Infraestructura.Dominio.Repositorio.UnidadTrabajo
{
    public class UnidadDeTrabajoEF : IUnidadDeTrabajo
    {
        private readonly AppDbContext _context;

        public UnidadDeTrabajoEF(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ITransaccion> IniciarTransaccionAsync()
        {
            var dbContextTransaction = await _context.Database.BeginTransactionAsync();
            return new TransaccionEF(dbContextTransaction);
        }

        public Task GuardarCambiosAsync() => _context.SaveChangesAsync();
    }

}
