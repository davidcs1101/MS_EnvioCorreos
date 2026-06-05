namespace ECO.Dominio.Repositorio.UnidadTrabajo
{
    public interface ITransaccion : IAsyncDisposable
    {
        Task CommitAsync();
        Task RollbackAsync();
    }
}
