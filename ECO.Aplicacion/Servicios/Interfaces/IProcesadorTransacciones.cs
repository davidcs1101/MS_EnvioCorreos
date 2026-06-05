namespace ECO.Aplicacion.Servicios.Interfaces
{
    public interface IProcesadorTransacciones
    {
        Task EjecutarEnTransaccionAsync(Func<Task> operacion);
    }
}
