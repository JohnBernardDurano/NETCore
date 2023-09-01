namespace SampleWebApiAspNetCore.Entities
{
    public class HoloENEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public int Generation { get; set; }
        public DateTime Created { get; set; }
    }
}
