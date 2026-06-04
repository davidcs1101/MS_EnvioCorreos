using ECO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.Aplicacion.ServiciosExternos
{
    public interface ICorreoServicio
    {
        Task<ApiResponse<string>> EnviarCorreoAsync(DatoCorreoRequest datosCorreo);
    }
}
