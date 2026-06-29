namespace ECO.Dtos
{
    public class PlantillaRequest
    {
        public string? Codigo { get; set; }
        public Dictionary<string, string> Variables { get; set; } = [];
    }
}
