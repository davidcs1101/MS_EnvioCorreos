namespace ECO.Dtos.AppSettings
{

    public class TrazabilidadCorreoSettings
    {
        public bool? GuardarDetalleCorreo { get; set; }
        public bool? GuardarAdjuntosCorreo { get; set; }
        public bool? GuardarEmlCorreo { get; set; }
    }

    public class TrabajosColasSettings
    {
        public string? ProcesarColaSolicitudesCron { get; set; }
        public string? CantidadIntentosPorRegistroEnCola { get; set; }
        public string? CantidadRegistrosProcesarIteracion { get; set; }
        public string? UsuarioIntegracion { get; set; }
        public string? ClaveIntegracion { get; set; }
    }

    public class ConfiguracionCorreoSettings
    {
        public string? Usuario { get; set; }
        public string? Clave { get; set; }
        public string? Host { get; set; }
        public string? Puerto { get; set; }
        public bool? UsaSsl { get; set; }
        public bool? UsaCredencialPorDefecto { get; set; }
        public string? CorreoRespuesta { get; set; }
    }



}
