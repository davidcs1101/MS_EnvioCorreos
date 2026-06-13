using ECO.Dominio.Entidades;

namespace ECO.Dominio.Repositorio
{
    public interface ICorreoEmlRepositorio
    {
        Task<int> CrearAsync(ECO_CorreoEml correoEml);
    }
}
