using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VidlyBackend.Models
{
    public class Movie
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title { get; set; }

        public int NumberInStock { get; set; }

        public int DailyRentalRate { get; set; }

        [BsonElement("Genre")]
        public Genre genre { get; set; }
    }
}
