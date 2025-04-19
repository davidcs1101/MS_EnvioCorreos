using ECO.Dtos;
namespace ECO.Aplicacion.Servicios.Interfaces
{
    public interface IApiResponse
    {
        ApiResponse<T> CrearRespuesta<T>(bool correcto, string mensaje, T? data = default);
    }
}
