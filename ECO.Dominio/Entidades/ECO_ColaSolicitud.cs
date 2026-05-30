using ECO.Dominio.Enumeraciones;
namespace ECO.Dominio.Entidades
{
    public class ECO_ColaSolicitud
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty;
        public EstadoCola Estado { get; set; }
        public int Intentos { get; set; } = 0;
        public DateTime FechaCreado { get; set; } = DateTime.Now;
        public DateTime? FechaUltimoIntento { get; set; }
        public string? ErrorMensaje { get; set; }
    }
}
