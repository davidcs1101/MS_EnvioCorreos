namespace ECO.Dominio.Entidades
{
    public class ECO_CorreoEml : ECO_BaseAuditoria
    {
        public int Id { get; set; }
        public ECO_Correo Correo { get; set; } = null!;
        public int CorreoId { get; set; }
        public string Nombre { get; set; } = null!;
        public long TamanoBytes { get; set; }
        public byte[] ContenidoArchivo { get; set; } = null!;
    }
}