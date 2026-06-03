namespace ECO.Dominio.Entidades
{
    public class ECO_CorreoAdjunto
    {
        public int Id { get; set; }
        public ECO_Correo Correo { get; set; } = null!;
        public int CorreoId { get; set; }
        public string Nombre { get; set; } = null!;
        public string Extension { get; set; } = null!;
        public string? TipoContenido { get; set; }
        public long TamanoBytes { get; set; }
        public byte[] ContenidoArchivo { get; set; } = null!;
        public DateTime FechaCreado { get; set; } = DateTime.UtcNow;
    }
}

/*
 foreach(var adjunto in request.ArchivosAdjuntos)
{
    var bytes = Convert.FromBase64String(adjunto.Contenido);

    var entidadAdjunto = new ECO_CorreoAdjunto
    {
        Nombre = adjunto.Nombre,
        Extension = adjunto.Extension,
        TamanoBytes = bytes.Length,
        ContenidoArchivo = bytes,
        TipoContenido = ObtenerMimeType(adjunto.Extension)
    };
}
 */