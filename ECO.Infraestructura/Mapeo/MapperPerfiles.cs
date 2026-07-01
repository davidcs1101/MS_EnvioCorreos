using ECO.Aplicacion.ServiciosExternos.Mapeo;
using ECO.Dominio.Entidades;
using ECO.Dtos;
using Riok.Mapperly.Abstractions;

namespace ECO.Infraestructura.Mapeo
{
    [Mapper]
    public partial class MapperPerfiles : IMapperPerfiles
    {
        public partial DatosCorreoDto CorreoCreacionRequestADatoCorreoDto(CorreoCreacionRequest datoCorreoRequest);
        public partial ECO_Correo CorreoCreacionRequestACorreo(CorreoCreacionRequest datoCorreoRequest);
        

        public partial ECO_Configuracion ConfiguracionCreacionRequestACorreoConfiguracion(ConfiguracionCreacionRequest correoConfiguracionCreacionRequest);
        public partial ConfiguracionDto ConfiguracionACorreoConfiguracionDto(ECO_Configuracion correoConfiguracion);
        public partial void ConfiguracionModificacionRequestACorreoConfiguracion(ConfiguracionModificacionRequest source, ECO_Configuracion target);


        public partial ECO_Plantilla PlantillaCreacionRequestAPlantilla(PlantillaCreacionRequest plantillaCreacionRequest);
        public partial PlantillaDto PlantillaAPlantillaDto(ECO_Plantilla plantilla);
        public partial void PlantillaModificacionRequestAPlantilla(PlantillaModificacionRequest source, ECO_Plantilla target);
    }
}
