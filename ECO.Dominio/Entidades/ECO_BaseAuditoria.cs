namespace ECO.Dominio.Entidades
{
    public class ECO_BaseAuditoria
    {
        public int UsuarioCreadorId { get; set; }
        public DateTime FechaCreado { get; set; } = DateTime.UtcNow;
    }
}
