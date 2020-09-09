using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VidlyBackend.Models;

namespace VidlyBackend.Dto
{
    public class UserReadDto
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Role { get; set; }
    }
}
