using ECO.Dominio.Entidades;

namespace ECO.Dominio.Repositorio
{
    public interface ICorreoRepositorio
    {
        void MarcarCrear(ECO_Correo correo);
        void MarcarModificar(ECO_Correo correo);
        Task<ECO_Correo?> ObtenerPorIdAsync(int id);

        Task ModificarAsync(ECO_Correo correo);    
    }
}
