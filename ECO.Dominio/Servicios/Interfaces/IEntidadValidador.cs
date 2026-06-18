using ECO.Dominio.Excepciones;

namespace ECO.Dominio.Servicios.Interfaces
{
    public interface IEntidadValidador<TEntity>
    {
        void ValidarDatoNoEncontrado(TEntity? entidad, string mensaje);
        void ValidarDatoYaExiste(TEntity? entidad, string mensaje);
    }
}
