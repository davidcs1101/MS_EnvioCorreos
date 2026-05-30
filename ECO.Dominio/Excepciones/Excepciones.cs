namespace ECO.Dominio.Excepciones
{
    public class DatoNoEncontradoException : Exception
    {
        public DatoNoEncontradoException(string mensaje) : base(mensaje) { }
    }

    public class SolicitudHttpException : Exception
    {
        public SolicitudHttpException(string mensaje) : base(mensaje) { }
    }

    public class LoguinException : Exception
    {
        public LoguinException(string mensaje) : base(mensaje) { }
    }
}
