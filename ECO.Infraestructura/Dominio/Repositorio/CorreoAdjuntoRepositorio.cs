using ECO.Dominio.Repositorio;
using ECO.DataAccess;
using ECO.Dominio.Entidades;

namespace ECO.Infraestructura.Dominio.Repositorio
{
    public class CorreoAdjuntoRepositorio : ICorreoAdjuntoRepositorio
    {
        private readonly AppDbContext _context;

        public CorreoAdjuntoRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public void MarcarCrear(ECO_CorreoAdjunto correoAdjunto)
        {
            _context.ECO_CorreoAdjuntos.Add(correoAdjunto);
        }
    }
}
