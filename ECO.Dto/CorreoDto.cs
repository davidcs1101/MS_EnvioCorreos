using ECO.Dominio.Enumeraciones;
namespace ECO.Dtos
{
    public class CorreoDto
    {
        public int Id { get; set; }
        public string Asunto { get; set; } = null!;
        public string Cuerpo { get; set; } = null!;
        public bool EsCuerpoHtml { get; set; }
        public string? CorreoRespuesta { get; set; }
        public EstadoCorreo Estado { get; set; }
        public string NombreEstado { get; set; } = null!;
        public string? ErrorMensaje { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public int? EmpresaId { get; set; }
        public string? CodigoPlantilla { get; set; }

        public List<CorreoAdjuntoDto> CorreosAdjuntos { get; set; } = new List<CorreoAdjuntoDto>();
        public List<CorreoDestinatarioDto> CorreosDestinatarios { get; set; } = new List<CorreoDestinatarioDto>();

        public DateTime FechaCreado { get; set; }
        public int? UsuarioCreadorId { get; set; }
    }
}
