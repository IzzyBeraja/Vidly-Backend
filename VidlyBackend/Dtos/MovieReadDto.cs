using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VidlyBackend.Dto
{
    public class MovieReadDto
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public int NumberInStock { get; set; }

        public int DailyRentalRate { get; set; }
    }
}
