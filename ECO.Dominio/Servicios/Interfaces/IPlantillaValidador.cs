using ECO.Dominio.Entidades;
using ECO.Dominio.Excepciones;

namespace ECO.Dominio.Servicios.Interfaces
{
    public interface IPlantillaValidador : IEntidadValidador<ECO_Plantilla>
    {
        void ValidarDatoActivo(bool estadoActivo, string mensaje);
    }
}
