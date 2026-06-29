using ECO.Dominio.Entidades;
using ECO.Dtos;

namespace ECO.Aplicacion.ServiciosExternos.Mapeo
{
    public interface IMapperPerfiles
    {
        DatosCorreoDto CorreoCreacionRequestADatoCorreoDto(CorreoCreacionRequest datoCorreoCreacionRequest);
        ECO_Correo CorreoCreacionRequestACorreo(CorreoCreacionRequest datoCorreoCreacionRequest);
 
        ECO_Configuracion ConfiguracionCreacionRequestACorreoConfiguracion(ConfiguracionCreacionRequest correoConfiguracionCreacionRequest);
        ConfiguracionDto ConfiguracionACorreoConfiguracionDto(ECO_Configuracion correoConfiguracion);
        //ECO_Configuracion ConfiguracionModificacionRequestACorreoConfiguracion(ConfiguracionModificacionRequest correoConfiguracionModificacionRequest);
        void ConfiguracionModificacionRequestACorreoConfiguracion(ConfiguracionModificacionRequest source, ECO_Configuracion target);
    }
}
