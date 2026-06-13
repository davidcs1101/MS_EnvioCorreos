namespace ECO.Dominio.Entidades
{
    public class ECO_CorreoAdjunto : ECO_BaseAuditoria
    {
        public int Id { get; set; }
        public ECO_Correo Correo { get; set; } = null!;
        public int CorreoId { get; set; }
        public string Nombre { get; set; } = null!;
        public string Extension { get; set; } = null!;
        public string? TipoContenido { get; set; }
        public long TamanoBytes { get; set; }
        public byte[] ContenidoArchivo { get; set; } = null!;
    }
}