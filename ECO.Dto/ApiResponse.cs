using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.Dtos
{
    public class ApiResponse<T>
    {
        public bool Correcto { get; set; }
        public string? Mensaje { get; set; }
        public T? Data { get; set; }
    }
}
