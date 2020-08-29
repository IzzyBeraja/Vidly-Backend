using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VidlyBackend.Models
{
    public class Rental
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public bool IsGold { get; set; }

        public string Phone { get; set; }
    }
}
