using ECO.Dominio.Repositorio.UnidadTrabajo;
using Microsoft.EntityFrameworkCore.Storage;

namespace ECO.Infraestructura.Dominio.Repositorio.UnidadTrabajo
{
    public class TransaccionEF : ITransaccion
    {
        private readonly IDbContextTransaction _transaccion;

        public TransaccionEF(IDbContextTransaction transaccion)
        {
            _transaccion = transaccion;
        }

        public Task CommitAsync() => _transaccion.CommitAsync();

        public Task RollbackAsync() => _transaccion.RollbackAsync();

        public ValueTask DisposeAsync() => _transaccion.DisposeAsync();
    }
}
