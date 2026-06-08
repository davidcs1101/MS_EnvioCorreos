using ECO.Dominio.Enumeraciones;
namespace ECO.Dominio.Entidades
{
    public class ECO_Correo
    {
        public int Id { get; set; }
        public string Asunto { get; set; } = null!;
        public string Cuerpo { get; set; } = null!;
        public bool EsCuerpoHtml { get; set; }
        public string? CorreoRespuesta { get; set; }
        public EstadoCorreo Estado { get; set; }
        public string? ErrorMensaje { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public int? EmpresaId { get; set; }
        public DateTime FechaCreado { get; set; } = DateTime.UtcNow;
        public List<ECO_CorreoDestinatario> CorreosDestinatarios { get; set; } = new List<ECO_CorreoDestinatario>();
        public List<ECO_CorreoAdjunto> CorreosAdjuntos { get; set; } = new List<ECO_CorreoAdjunto>();
        public List<ECO_CorreoEml> CorreosEml { get; set; } = new List<ECO_CorreoEml>();
    }
}
