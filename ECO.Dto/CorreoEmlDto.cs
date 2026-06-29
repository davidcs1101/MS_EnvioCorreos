namespace ECO.Dtos
{
    public class CorreoEmlDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public long TamanoBytes { get; set; }
        public byte[] ContenidoArchivo { get; set; } = null!;

        public DateTime FechaCreado { get; set; }
        public int? UsuarioCreadorId { get; set; }
    }
}
