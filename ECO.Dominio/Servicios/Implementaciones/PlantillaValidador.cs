using ECO.Dominio.Entidades;
using ECO.Dominio.Excepciones;
using ECO.Dominio.Servicios.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ECO.Dominio.Servicios.Implementaciones
{
    public class PlantillaValidador : IPlantillaValidador
    {
        public void ValidarDatoNoEncontrado(ECO_Plantilla? entidad, string mensaje)
        {
            if (entidad == null)
                throw new DatoNoEncontradoException(mensaje);
        }
        public void ValidarDatoYaExiste(ECO_Plantilla? entidad, string mensaje)
        {
            if (entidad != null)
                throw new DatoYaExisteException(mensaje);
        }
        public void ValidarDatoActivo(bool estadoActivo, string mensaje)
        {
            if (!estadoActivo)
                throw new DatoInactivoException(mensaje);
        }
    }
}
