using ECO.Dominio.Repositorio;
using ECO.DataAccess;
using ECO.Dominio.Entidades;

namespace ECO.Infraestructura.Dominio.Repositorio
{
    public class CorreoDestinatarioRepositorio : ICorreoDestinatarioRepositorio
    {
        private readonly AppDbContext _context;

        public CorreoDestinatarioRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public void MarcarCrear(ECO_CorreoDestinatario correoDestinatario)
        {
            _context.ECO_CorreosDestinatarios.Add(correoDestinatario);
        }
    }
}
