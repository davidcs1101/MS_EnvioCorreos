using ECO.Dominio.Enumeraciones;
namespace ECO.Dominio.Entidades
{
    public class ECO_CorreoDestinatario
    {
        public int Id { get; set; }
        public ECO_Correo Correo { get; set; } = null!;
        public int CorreoId { get; set; }
        public string Destinatario { get; set; } = null!;
        public TipoDestinatario Tipo { get; set; }
        public DateTime FechaCreado { get; set; } = DateTime.UtcNow;
    }
}
