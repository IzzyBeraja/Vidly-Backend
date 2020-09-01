using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VidlyBackend.Models;

namespace VidlyBackend.Dto
{
    public class MovieReadDto
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public int NumberInStock { get; set; }

        public int DailyRentalRate { get; set; }

        public Genre genre { get; set; }
    }
}
