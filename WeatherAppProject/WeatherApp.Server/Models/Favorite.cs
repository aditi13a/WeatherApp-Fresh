namespace WeatherApp.Server.Models
{
    public class Favorite
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string City { get; set; } = string.Empty;
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
