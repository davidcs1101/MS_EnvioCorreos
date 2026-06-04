using ECO.Aplicacion.ServiciosExternos.Mapeo;
using ECO.Dtos;
using Riok.Mapperly.Abstractions;

namespace ECO.Infraestructura.Mapeo
{
    [Mapper]
    public partial class MapperPerfiles : IMapperPerfiles
    {
        public partial DatoCorreoDto DatoCorreoRequestADatoCorreoDto(DatoCorreoRequest datoCorreoRequest);
    }
}
