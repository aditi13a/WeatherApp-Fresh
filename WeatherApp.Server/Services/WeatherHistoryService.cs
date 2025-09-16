using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace WeatherApp.Server.Services
{
    public class WeatherHistoryService
    {
        private readonly IMongoCollection<WeatherHistory> _weatherCollection;

        public WeatherHistoryService(IConfiguration configuration)
        {
            var mongoClient = new MongoClient(configuration.GetConnectionString("MongoDb"));
            var mongoDatabase = mongoClient.GetDatabase("WeatherAppDb");
            _weatherCollection = mongoDatabase.GetCollection<WeatherHistory>("WeatherHistory");
        }

        public async Task<List<WeatherHistory>> GetAsync() =>
            await _weatherCollection.Find(_ => true).SortByDescending(x => x.TimeSearched).ToListAsync();

        public async Task<WeatherHistory?> GetByIdAsync(string id) =>
            await _weatherCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(WeatherHistory weatherHistory) =>
            await _weatherCollection.InsertOneAsync(weatherHistory);

        public async Task UpdateAsync(string id, WeatherHistory updatedWeatherHistory) =>
            await _weatherCollection.ReplaceOneAsync(x => x.Id == id, updatedWeatherHistory);

        public async Task DeleteAsync(string id) =>
            await _weatherCollection.DeleteOneAsync(x => x.Id == id);
    }

    public class WeatherHistory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public DateTime TimeSearched { get; set; } = DateTime.UtcNow;
    }
}
