using System.ComponentModel.DataAnnotations;

namespace VidlyBackend.Dto
{
    public class GenreCreateDto
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
