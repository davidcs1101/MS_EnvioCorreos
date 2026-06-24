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
        public bool Estado { get; set; } = true;
    }
}