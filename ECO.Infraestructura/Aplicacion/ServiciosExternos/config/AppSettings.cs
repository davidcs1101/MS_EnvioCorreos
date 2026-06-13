using Microsoft.Extensions.Options;
using ECO.Aplicacion.ServiciosExternos.config;
using ECO.Dtos.AppSettings;
namespace ECO.Infraestructura.Aplicacion.ServiciosExternos.Config
{
    public class AppSettings : IAppSettings
    {
        private readonly TrabajosColasSettings _trabajosColas;
        private readonly TrazabilidadCorreoSettings _trazabilidadCorreo;
        private readonly ConfiguracionCorreoSettings _configuracionCorreo;

        public AppSettings(
            IOptions<TrabajosColasSettings> opcionesTrabajosColas, 
            IOptions<TrazabilidadCorreoSettings> trazabilidadCorreo, 
            IOptions<ConfiguracionCorreoSettings> configuracionCorreo)  
        {
            _trabajosColas = opcionesTrabajosColas.Value;
            _trazabilidadCorreo = trazabilidadCorreo.Value;
            _configuracionCorreo = configuracionCorreo.Value;
        }

        //TrabajosColas
        public TrabajosColasSettings ObtenerTrabajosColasSettings() 
        {
            return new TrabajosColasSettings
            {
                CantidadIntentosPorRegistroEnCola = _trabajosColas.CantidadIntentosPorRegistroEnCola,

                CantidadRegistrosProcesarIteracion = _trabajosColas.CantidadRegistrosProcesarIteracion,

                ProcesarColaSolicitudesCron =
                    string.IsNullOrWhiteSpace(_trabajosColas.ProcesarColaSolicitudesCron)
                        ? "*/5 * * * *"
                        : _trabajosColas.ProcesarColaSolicitudesCron,

                UsuarioIntegracion = _trabajosColas.UsuarioIntegracion,

                ClaveIntegracion = _trabajosColas.ClaveIntegracion
            };
        }

        //NivelTrazabilidadCorreo
        public TrazabilidadCorreoSettings ObtenerTrazabilidadCorreoSettings() {
            return _trazabilidadCorreo;
        }

        //ConfiguracionCorreo
        public ConfiguracionCorreoSettings ObtenerConfiguracionCorreoSettings()
        {
            return new ConfiguracionCorreoSettings
            {
                Usuario = _configuracionCorreo.Usuario,
                Clave = _configuracionCorreo.Clave,
                Host = _configuracionCorreo.Host,
                Puerto = _configuracionCorreo.Puerto,
                UsaSsl = _configuracionCorreo.UsaSsl,
                UsaCredencialPorDefecto = _configuracionCorreo.UsaCredencialPorDefecto,
                CorreoRespuesta = _configuracionCorreo.CorreoRespuesta
            };
        }

    }
}
