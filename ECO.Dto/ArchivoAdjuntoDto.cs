using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.Dtos
{
    public class ArchivoAdjuntoDto
    {
        [Required(ErrorMessage = "El dato es obligatorio")]
        public string Nombre { get; set; } = null!;
        [Required(ErrorMessage = "El dato es obligatorio")]
        public string Extension { get; set; } = null!;
        [Required(ErrorMessage = "El dato es obligatorio")]
        public string Contenido { get; set; } = null!;
    }
}
