using Utilidades.AtributosValidaciones;

namespace ECO.Dtos
{
    public class AccionesDto
    {
        public bool GuardarDetalleCorreo { get; set; } = false;

        [RequeridoSiAtributoEsIgualA(nameof(GuardarDetalleCorreo), true, ErrorMessage = "El dato es obligatorio si el atributo " + nameof(GuardarDetalleCorreo) + " es true")]
        public bool GuardarAdjuntosCorreo { get; set; } = false;
    }
}
