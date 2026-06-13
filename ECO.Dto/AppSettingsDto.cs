namespace ECO.Dtos.AppSettings
{

    public class TrazabilidadCorreoSettings
    {
        public bool GuardarDetalleCorreo { get; set; }
        public bool GuardarAdjuntosCorreo { get; set; }
        public bool GuardarEmlCorreo { get; set; }
    }

    public class TrabajosColasSettings
    {
        public int CantidadIntentosPorRegistroEnCola { get; set; } = 0; //Cantidad de Reintentos para procesar un registro en la cola antes de marcarlo como error.
        public string ProcesarColaSolicitudesCron { get; set; } = "*/5 * * * *";
        public int CantidadRegistrosProcesarIteracion { get; set; } = 10;
        public string UsuarioIntegracion { get; set; } = "";
        public string ClaveIntegracion { get; set; } = "";
    }

    public class ConfiguracionCorreoSettings
    {
        public string Usuario { get; set; } = "";
        public string Clave { get; set; } = "";
        public string Host { get; set; } = "";
        public int Puerto { get; set; }
        public bool UsaSsl { get; set; }
        public bool UsaCredencialPorDefecto { get; set; }
        public string CorreoRespuesta { get; set; } = "";
    }



}
