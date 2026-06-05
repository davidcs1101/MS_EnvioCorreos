namespace ECO.Aplicacion.ServiciosExternos
{
    public interface ISerializadorJsonServicio
    {
        string Serializar<T>(T objeto);
        T Deserializar<T>(string json);
    }
}
