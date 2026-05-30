using ECO.Dominio.Entidades;
using ECO.Dominio.Excepciones;
using ECO.Dominio.Servicios.Interfaces;

namespace ECO.Dominio.Servicios.Implementaciones
{
    public class ColaSolicitudValidador : IColaSolicitudValidador
    {
        public void ValidarDatoNoEncontrado(ECO_ColaSolicitud? cola, string mensaje)
        {
            if (cola == null)
                throw new DatoNoEncontradoException(mensaje);
        }
    }
}
