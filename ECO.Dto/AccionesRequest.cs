using Utilidades.AtributosValidaciones;

namespace ECO.Dtos
{
    public class AccionesRequest
    {
        public bool GuardarDetalleCorreo { get; set; } = false;
        public bool GuardarAdjuntosCorreo { get; set; } = false;
        public bool GuardarEmlCorreo { get; set; } = false;
    }
}
