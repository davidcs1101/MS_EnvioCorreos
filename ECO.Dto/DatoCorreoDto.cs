using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilidades;

namespace ECO.Dtos
{
    public class DatoCorreoDto
    {
        [Required(ErrorMessage = Textos.Generales.VALIDA_CAMPO_OBLIGATORIO)]
        [CustomValidation(typeof(DatoCorreoDto),nameof(ValidarCorreos))]
        public List<string> Destinatarios { get; set; } = new List<string>();
        [CustomValidation(typeof(DatoCorreoDto), nameof(ValidarCorreos))]
        public List<string> CC { get; set; } = new List<string>();
        [CustomValidation(typeof(DatoCorreoDto), nameof(ValidarCorreos))]
        public List<string> CCO { get; set; } = new List<string>();

        [EmailAddress(ErrorMessage = Textos.Generales.VALIDA_CORREO_NO_VALIDO)]
        public string? CorreoRespuesta { get; set; }
        
        [Required(ErrorMessage = Textos.Generales.VALIDA_CAMPO_OBLIGATORIO)]
        public string Asunto { get; set; } = null!;
        [Required(ErrorMessage = Textos.Generales.VALIDA_CAMPO_OBLIGATORIO)]
        public string Cuerpo { get; set; } = null!;
        [Required(ErrorMessage = Textos.Generales.VALIDA_CAMPO_OBLIGATORIO)]
        public bool esCuerpoHtml { get; set; }
        public List<ArchivoAdjuntoDto> ArchivosAdjuntos { get; set; } = new List<ArchivoAdjuntoDto>();


        public static ValidationResult? ValidarCorreos(List<string> correos, ValidationContext contexto) 
        {
            var atributosCorreo = new EmailAddressAttribute();
            foreach (var correo in correos)
            {
                if (!atributosCorreo.IsValid(correo))
                    return new ValidationResult(Textos.Generales.VALIDA_CORREO_NO_VALIDO + " _ " + correo);
            }
            return ValidationResult.Success;
        }
    }
}
