using System.ComponentModel.DataAnnotations;
using Utilidades;

namespace ECO.Dtos
{
    public class DatosCorreoEmpresaRequest : DatosCorreoRequest
    {
        [Required(ErrorMessage = Textos.Generales.VALIDA_CAMPO_OBLIGATORIO)]
        public int EmpresaId { get; set; }
    }
}
