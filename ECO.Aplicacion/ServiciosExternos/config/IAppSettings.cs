namespace ECO.Aplicacion.ServiciosExternos.config
{
    public interface IAppSettings
    {
        //TrabajosColas
        int ObtenerCantidadIntentosPorRegistroEnCola();
        string ObtenerProcesarColaSolicitudesCron();
        int ObtenerCantidadRegistrosProcesarIteracion();
        string ObtenerUsuarioIntegracion();
        string ObtenerClaveIntegracion();


        //NivelTrazabilidadCorreo
        bool ObtenerGuardarDetalleCorreo();
        bool ObtenerGuardarAdjuntosCorreo();
        bool ObtenerGuardarEmlCorreo();


        //ConfiguracionCorreo
        string ObtenerUsuarioCorreo();
        string ObtenerClaveCorreo();
        string ObtenerHostCorreo();
        int ObtenerPuertoCorreo();
        bool ObtenerUsaSslCorreo();
        bool ObtenerUsaCredencialPorDefectoCorreo();
        string ObtenerCorreoRespuesta();

    }
}
