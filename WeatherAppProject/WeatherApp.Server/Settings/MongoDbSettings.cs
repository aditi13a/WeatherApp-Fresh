namespace WeatherApp.Server.Settings
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string WeatherCollectionName { get; set; } = null!;
    }
}
