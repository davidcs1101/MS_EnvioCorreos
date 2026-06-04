using ECO.Dtos;

namespace ECO.Aplicacion.ServiciosExternos.Mapeo
{
    public interface IMapperPerfiles
    {
        DatoCorreoDto DatoCorreoRequestADatoCorreoDto(DatoCorreoRequest datoCorreoRequest);
    }
}
