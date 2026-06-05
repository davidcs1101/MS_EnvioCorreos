namespace ECO.Aplicacion.ServiciosExternos.config
{
    public interface IConfiguracionesTrabajosColas
    {
        int ObtenerCantidadIntentosPorRegistroEnCola();
        string ObtenerProcesarColaSolicitudesCron();
        int ObtenerCantidadRegistrosProcesarIteracion();
        string ObtenerUsuarioIntegracion();
        string ObtenerClaveIntegracion();
    }
}
