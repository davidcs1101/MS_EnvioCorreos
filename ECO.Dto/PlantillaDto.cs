using System.ComponentModel.DataAnnotations;
using Utilidades;

namespace ECO.Dtos
{
    public class PlantillaDto
    {
        public int Id { get; set; }
        public int EmpresaId { get; set; }
        public string Codigo { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Asunto { get; set; } = null!;
        public string Html { get; set; } = null!;

        public bool EstadoActivo { get; set; }
        public DateTime FechaCreado { get; set; }
        public int UsuarioCreadorId { get; set; }
        public DateTime? FechaModificado { get; set; }
        public int? UsuarioModificadoId { get; set; }
    }
}
