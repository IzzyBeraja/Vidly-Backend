using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VidlyBackend.Models
{
    public class MovieModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Title")]
        public string Title { get; set; }

        [BsonElement("NumberInStock")]
        public int NumberInStock { get; set; }

        [BsonElement("DailyRentalRate")]
        public int DailyRentalRate { get; set; }
    }
}
