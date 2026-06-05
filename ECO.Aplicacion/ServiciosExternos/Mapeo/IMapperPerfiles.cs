using ECO.Dominio.Entidades;
using ECO.Dtos;

namespace ECO.Aplicacion.ServiciosExternos.Mapeo
{
    public interface IMapperPerfiles
    {
        DatosCorreoDto DatoCorreoRequestADatoCorreoDto(DatosCorreoRequest datoCorreoRequest);
        ECO_Correo DatoCorreoRequestACorreo(DatosCorreoRequest datoCorreoRequest);
    }
}
