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
        void ConfiguracionModificacionRequestACorreoConfiguracion(ConfiguracionModificacionRequest source, ECO_Configuracion target);


        ECO_Plantilla PlantillaCreacionRequestAPlantilla(PlantillaCreacionRequest plantillaCreacionRequest);
        PlantillaDto PlantillaAPlantillaDto(ECO_Plantilla plantilla);
        void PlantillaModificacionRequestAPlantilla(PlantillaModificacionRequest source, ECO_Plantilla target);

    }
}
