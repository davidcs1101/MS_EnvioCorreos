namespace ECO.Aplicacion.ServiciosExternos
{
    public interface IUsuarioContextoServicio
    {
        int ObtenerUsuarioIdToken();
        int ObtenerEmpresaIdToken();
        int ValidarEmpresaIdToken(int empresaIdBody);
    }
}
