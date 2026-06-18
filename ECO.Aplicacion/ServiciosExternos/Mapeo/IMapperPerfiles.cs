using ECO.Dominio.Entidades;
using ECO.Dtos;

namespace ECO.Aplicacion.ServiciosExternos.Mapeo
{
    public interface IMapperPerfiles
    {
        DatosCorreoRequest DatoCorreoRequestADatoCorreoDto(CorreoCreacionRequest datoCorreoRequest);
        ECO_Correo DatoCorreoRequestACorreo(CorreoCreacionRequest datoCorreoRequest);
        ECO_CorreoConfiguracion CorreoConfiguracionCreacionRequestACorreoConfiguracion(CorreoConfiguracionCreacionRequest correoConfiguracionCreacionRequest);
        CorreoConfiguracionDto CorreoConfiguracionACorreoConfiguracionDto(ECO_CorreoConfiguracion correoConfiguracion);
    }
}
