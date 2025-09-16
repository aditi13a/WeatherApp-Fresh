using MongoDB.Driver;
using WeatherApp.Server.Models;

namespace WeatherApp.Server.Services
{
    public class FavoritesService
    {
        private readonly IMongoCollection<Favorite> _favorites;

        public FavoritesService(IConfiguration config)
        {
            // key **must** match appsettings.json
            var client = new MongoClient(
                config.GetConnectionString("MongoDb"));   // ← match name
            var database = client.GetDatabase("MongoDb");
            _favorites = database.GetCollection<Favorite>("Favorites");
        }

        public async Task<List<Favorite>> GetAsync() =>
            await _favorites.Find(_ => true).ToListAsync();

        public async Task CreateAsync(Favorite favorite) =>
            await _favorites.InsertOneAsync(favorite);

        public async Task DeleteAsync(string id) =>
            await _favorites.DeleteOneAsync(f => f.Id == id);

        // after UpdateAsync / DeleteAsync
        public async Task<Favorite?> GetByCityAsync(string city) =>
            await _favorites.Find(x => x.City.ToLower() == city.ToLower())
                            .FirstOrDefaultAsync();

    }
}
