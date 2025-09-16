using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WeatherApp.Server.Models
{
    public class WeatherHistory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string City { get; set; } = null!;
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
