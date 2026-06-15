using ECO.Dominio.Enumeraciones;

namespace ECO.Dtos
{
    public class CorreoDestinatarioDto
    {
        public string Destinatario { get; set; } = null!;
        public TipoDestinatario Tipo { get; set; }
        public string NombreTipo { get; set; } = null!;
    }
}
