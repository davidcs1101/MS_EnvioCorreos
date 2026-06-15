using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilidades;
using Utilidades.AtributosValidaciones;

namespace ECO.Dtos
{
    public class CorreoCreacionRequest
    {
        [ListaDebeContenerElementos(ErrorMessage = "Debe existir al menos un destinatario.")]
        [ListaCorreosValidos]
        public List<string> Destinatarios { get; set; } = new List<string>();

        [ListaCorreosValidos]
        public List<string> CC { get; set; } = new List<string>();

        [ListaCorreosValidos]
        public List<string> CCO { get; set; } = new List<string>();

        [EmailAddress(ErrorMessage = Textos.Generales.VALIDA_CORREO_NO_VALIDO)]
        public string? CorreoRespuesta { get; set; }
        
        [Required(ErrorMessage = Textos.Generales.VALIDA_CAMPO_OBLIGATORIO)]
        public string Asunto { get; set; } = null!;

        [Required(ErrorMessage = Textos.Generales.VALIDA_CAMPO_OBLIGATORIO)]
        public string Cuerpo { get; set; } = null!;

        [Required(ErrorMessage = Textos.Generales.VALIDA_CAMPO_OBLIGATORIO)]
        public bool EsCuerpoHtml { get; set; }

        public List<CorreoAdjuntoRequest> ArchivosAdjuntos { get; set; } = new List<CorreoAdjuntoRequest>();

        public AccionesRequest Acciones { get; set; } = new AccionesRequest();

        [Required(ErrorMessage = Textos.Generales.VALIDA_CAMPO_OBLIGATORIO)]
        public int UsuarioCreadorId { get; set; }
    }
}
