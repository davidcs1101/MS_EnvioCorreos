using ECO.Dtos.AppSettings;

namespace ECO.Aplicacion.ServiciosExternos.config
{
    public interface IAppSettings
    {
        //TrabajosColas
        TrabajosColasSettings ObtenerTrabajosColasSettings();

        //NivelTrazabilidadCorreo
        TrazabilidadCorreoSettings ObtenerTrazabilidadCorreoSettings();

        //ConfiguracionCorreo
        ConfiguracionCorreoSettings ObtenerConfiguracionCorreoSettings();
    }
}
