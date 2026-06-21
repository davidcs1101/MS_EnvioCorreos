using ECO.Dominio.Enumeraciones;
namespace ECO.Dominio.Entidades
{
    public class ECO_Correo : ECO_BaseAuditoria
    {
        public int Id { get; set; }
        public Guid Codigo { get; set; } = Guid.NewGuid();
        public string Asunto { get; set; } = null!;
        public string Cuerpo { get; set; } = null!;
        public bool EsCuerpoHtml { get; set; }
        public string? CorreoRespuesta { get; set; }
        public EstadoCorreo Estado { get; set; }
        public string? ErrorMensaje { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public int? EmpresaId { get; set; }
        public List<ECO_CorreoDestinatario> CorreosDestinatarios { get; set; } = new();
        public List<ECO_CorreoAdjunto> CorreosAdjuntos { get; set; } = new();
        public ECO_CorreoEml? CorreoEml { get; set; }
    }
}
