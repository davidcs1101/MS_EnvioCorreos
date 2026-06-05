using Microsoft.Extensions.Options;
using ECO.Aplicacion.ServiciosExternos.config;
using ECO.Dtos.AppSettings;
namespace ECO.Infraestructura.Aplicacion.ServiciosExternos.Config
{
    public class ConfiguracionesTrabajosColas : IConfiguracionesTrabajosColas
    {
        private readonly TrabajosColasSettings _opciones;

        public ConfiguracionesTrabajosColas(IOptions<TrabajosColasSettings> opciones)
        {
            _opciones = opciones.Value;
        }

        public int ObtenerCantidadIntentosPorRegistroEnCola()
        {
            if (int.TryParse(_opciones.CantidadIntentosPorRegistroEnCola, out int dato))
                return dato;
            return 0;
        }

        public string ObtenerProcesarColaSolicitudesCron()
        {
            return string.IsNullOrWhiteSpace(_opciones.ProcesarColaSolicitudesCron)
                            ? "*/5 * * * *"
                            : _opciones.ProcesarColaSolicitudesCron;
        }

        public int ObtenerCantidadRegistrosProcesarIteracion()
        {
            if (int.TryParse(_opciones.CantidadRegistrosProcesarIteracion, out int dato))
                return dato;
            return 0;
        }

        public string ObtenerUsuarioIntegracion()
        {
            return string.IsNullOrWhiteSpace(_opciones.UsuarioIntegracion) ? "" : _opciones.UsuarioIntegracion;
        }

        public string ObtenerClaveIntegracion()
        {
            return string.IsNullOrWhiteSpace(_opciones.ClaveIntegracion) ? "" : _opciones.ClaveIntegracion;
        }
    }
}
