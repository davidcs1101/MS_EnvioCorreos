namespace ECO.Dominio.Enumeraciones
{
    public enum EstadoCola
    {
        Pendiente = 0,
        Procesando = 1,
        Exitoso = 2,
        Fallido = 3
    }

    public enum EstadoCorreo
    {
        Pendiente = 0,
        Enviado = 1,
        Fallido = 2
    }

    public enum TipoDestinatario
    {
        Para = 0,
        CC = 1,
        CCO = 2
    }
}
