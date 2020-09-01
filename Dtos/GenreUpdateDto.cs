using System.ComponentModel.DataAnnotations;

namespace VidlyBackend.Dto
{
    public class GenreUpdateDto
    {
        [Required]
        public string Name { get; set; }
    }
}
