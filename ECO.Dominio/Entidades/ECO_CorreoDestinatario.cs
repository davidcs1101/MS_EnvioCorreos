using ECO.Dominio.Enumeraciones;
namespace ECO.Dominio.Entidades
{
    public class ECO_CorreoDestinatario : ECO_BaseAuditoria
    {
        public int Id { get; set; }
        public ECO_Correo Correo { get; set; } = null!;
        public int CorreoId { get; set; }
        public string Destinatario { get; set; } = null!;
        public TipoDestinatario Tipo { get; set; }
    }
}
