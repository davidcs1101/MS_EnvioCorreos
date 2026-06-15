using ECO.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.Dtos
{
    public class CorreoAdjuntoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Extension { get; set; } = null!;
        public string? TipoContenido { get; set; }
        public long TamanoBytes { get; set; }
        public byte[] ContenidoArchivo { get; set; } = null!;
        public string ContenidoArchivoB64 { get; set; } = null!;
    }
}
