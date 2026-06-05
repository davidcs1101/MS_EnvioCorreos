using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ECO.Aplicacion.ServiciosExternos;
using ECO.Aplicacion.ServiciosExternos.config;
using ECO.Dtos.AppSettings;
namespace ECO.Infraestructura.Aplicacion.ServiciosExternos.Config
{
    public class ConfiguracionesTrazabilidadCorreo: IConfiguracionesTrazabilidadCorreo
    {
        private readonly TrazabilidadCorreoSettings _opciones;
        private readonly ISerializadorJsonServicio _serializadorJsonServicio;

        public ConfiguracionesTrazabilidadCorreo(IOptions<TrazabilidadCorreoSettings> opciones, IServiceProvider serviceProvider, ISerializadorJsonServicio serializadorJsonServicio)
        {
            _opciones = opciones.Value;
            _serializadorJsonServicio = serializadorJsonServicio;
        }

        public bool ObtenerGuardarDetalleCorreo()
        {
            return _opciones.GuardarDetalleCorreo ?? false;
        }

        public bool ObtenerGuardarAdjuntosCorreo()
        {
            return _opciones.GuardarAdjuntosCorreo ?? false;
        }

        public bool ObtenerGuardarEmlCorreo()
        {
            return _opciones.GuardarEmlCorreo ?? false;
        }
    }
}
