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
        public int ObtenerCantidadIntentosPorRegistroEnCola()
        {
            if (int.TryParse(_trabajosColas.CantidadIntentosPorRegistroEnCola, out int dato))
                return dato;
            return 0;
        }
        public string ObtenerProcesarColaSolicitudesCron()
        {
            return string.IsNullOrWhiteSpace(_trabajosColas.ProcesarColaSolicitudesCron)
                            ? "*/5 * * * *"
                            : _trabajosColas.ProcesarColaSolicitudesCron;
        }
        public int ObtenerCantidadRegistrosProcesarIteracion()
        {
            if (int.TryParse(_trabajosColas.CantidadRegistrosProcesarIteracion, out int dato))
                return dato;
            return 0;
        }
        public string ObtenerUsuarioIntegracion()
        {
            return string.IsNullOrWhiteSpace(_trabajosColas.UsuarioIntegracion) 
                ? "" : _trabajosColas.UsuarioIntegracion;
        }
        public string ObtenerClaveIntegracion()
        {
            return string.IsNullOrWhiteSpace(_trabajosColas.ClaveIntegracion) 
                ? "" : _trabajosColas.ClaveIntegracion;
        }


        //NivelTrazabilidadCorreo
        public bool ObtenerGuardarDetalleCorreo()
        {
            return _trazabilidadCorreo.GuardarDetalleCorreo ?? false;
        }
        public bool ObtenerGuardarAdjuntosCorreo()
        {
            return _trazabilidadCorreo.GuardarAdjuntosCorreo ?? false;
        }
        public bool ObtenerGuardarEmlCorreo()
        {
            return _trazabilidadCorreo.GuardarEmlCorreo ?? false;
        }


        //ConfiguracionCorreo
        public string ObtenerUsuarioCorreo()
        {
            return string.IsNullOrWhiteSpace(_configuracionCorreo.Usuario) ? "" : _configuracionCorreo.Usuario;
        }
        public string ObtenerClaveCorreo()
        {
            return string.IsNullOrWhiteSpace(_configuracionCorreo.Clave) ? "" : _configuracionCorreo.Clave;
        }
        public string ObtenerHostCorreo()
        {
            return string.IsNullOrWhiteSpace(_configuracionCorreo.Host) ? "" : _configuracionCorreo.Host;
        }
        public int ObtenerPuertoCorreo()
        {
            if (int.TryParse(_configuracionCorreo.Puerto, out int puerto))
                return puerto;
            return 0;
        }
        public bool ObtenerUsaSslCorreo()
        {
            return _configuracionCorreo.UsaSsl ?? false;
        }
        public bool ObtenerUsaCredencialPorDefectoCorreo()
        {
            return _configuracionCorreo.UsaCredencialPorDefecto ?? false;
        } 
        public string ObtenerCorreoRespuesta()
        {
            return string.IsNullOrWhiteSpace(_configuracionCorreo.CorreoRespuesta) ? "" : _configuracionCorreo.CorreoRespuesta;
        }

    }
}
