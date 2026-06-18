using ECO.Aplicacion.ServiciosExternos.Mapeo;
using ECO.Dominio.Entidades;
using ECO.Dtos;
using Riok.Mapperly.Abstractions;

namespace ECO.Infraestructura.Mapeo
{
    [Mapper]
    public partial class MapperPerfiles : IMapperPerfiles
    {
        public partial DatosCorreoRequest DatoCorreoRequestADatoCorreoDto(CorreoCreacionRequest datoCorreoRequest);
        public partial ECO_Correo DatoCorreoRequestACorreo(CorreoCreacionRequest datoCorreoRequest);
        public partial ECO_CorreoConfiguracion CorreoConfiguracionCreacionRequestACorreoConfiguracion(CorreoConfiguracionCreacionRequest correoConfiguracionCreacionRequest);
        public partial CorreoConfiguracionDto CorreoConfiguracionACorreoConfiguracionDto(ECO_CorreoConfiguracion correoConfiguracion);
    }
}
