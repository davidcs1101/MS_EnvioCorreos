using ECO.Dominio.Excepciones;
using ECO.Dominio.Servicios.Interfaces;

namespace ECO.Dominio.Servicios.Implementaciones
{
    public class EntidadValidador<TEntity> : IEntidadValidador<TEntity>
    {
        public void ValidarDatoNoEncontrado(TEntity? entidad, string mensaje)
        {
            if (entidad == null)
                throw new DatoNoEncontradoException(mensaje);
        }
    }
}
