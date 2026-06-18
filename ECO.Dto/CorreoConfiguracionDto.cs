using ECO.Dominio.Enumeraciones;
namespace ECO.Dtos
{
    public class CorreoConfiguracionDto
    {
        public int Id { get; set; }
        public int EmpresaId { get; set; }
        public string Codigo { get; set; } = null!;
        public string Descripcion { get; set; } = null!;

        public string Usuario { get; set; } = null!;
        public string Clave { get; set; } = null!;
        public string Host { get; set; } = null!;
        public int Puerto { get; set; }
        public bool UsaSsl { get; set; }
        public bool UsaCredencialPorDefecto { get; set; }
        public string CorreoRespuesta { get; set; } = null!;
        public bool Activo { get; set; }

        public DateTime FechaCreado { get; set; }
        public int UsuarioCreadorId { get; set; }
    }
}
