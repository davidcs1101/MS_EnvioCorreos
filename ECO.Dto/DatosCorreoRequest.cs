using System.ComponentModel.DataAnnotations;
using Utilidades;
using Utilidades.AtributosValidaciones;

namespace ECO.Dtos
{
    public class DatosCorreoRequest
    {
        public List<string> Destinatarios { get; set; } = new List<string>();
        public List<string> CC { get; set; } = new List<string>();
        public List<string> CCO { get; set; } = new List<string>();
        public string? CorreoRespuesta { get; set; }
        public string Asunto { get; set; } = null!;
        public string Cuerpo { get; set; } = null!;
        public bool EsCuerpoHtml { get; set; }
        public List<CorreoAdjuntoRequest> ArchivosAdjuntos { get; set; } = new List<CorreoAdjuntoRequest>();
        public AccionesRequest Acciones { get; set; } = new AccionesRequest();
        public int CorreoId { get; set; }
    }
}
