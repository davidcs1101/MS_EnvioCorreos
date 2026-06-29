namespace ECO.Dominio.Entidades
{
    public class ECO_Plantilla : ECO_BaseAuditoria
    {
        public int Id { get; set; }
        public int EmpresaId { get; set; }
        public string Codigo { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Asunto { get; set; } = null!;
        public string Html { get; set; } = null!;
        public bool EstadoActivo { get; set; } = true;
        public List<ECO_Correo> Correos { get; set; } = new();
        public int? UsuarioModificadorId { get; set; }
        public DateTime? FechaModificado { get; set; }
    }
}