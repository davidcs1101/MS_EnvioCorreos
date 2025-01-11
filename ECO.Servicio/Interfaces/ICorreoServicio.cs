using ECO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.Servicio.Interfaces
{
    public interface ICorreoServicio
    {
        Task<ApiResponse<string>> EnviarCorreoAsync(DatoCorreoDto datosCorreo);
    }
}
