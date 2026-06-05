using ECO.Aplicacion.ServiciosExternos.Mapeo;
using ECO.Dominio.Entidades;
using ECO.Dtos;
using Riok.Mapperly.Abstractions;

namespace ECO.Infraestructura.Mapeo
{
    [Mapper]
    public partial class MapperPerfiles : IMapperPerfiles
    {
        public partial DatosCorreoDto DatoCorreoRequestADatoCorreoDto(DatosCorreoRequest datoCorreoRequest);
        public partial ECO_Correo DatoCorreoRequestACorreo(DatosCorreoRequest datoCorreoRequest);
    }
}
