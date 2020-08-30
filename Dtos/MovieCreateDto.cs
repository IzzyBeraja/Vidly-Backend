using System.ComponentModel.DataAnnotations;

namespace VidlyBackend.Dto
{
    public class MovieCreateDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public int? NumberInStock { get; set; }

        [Required]
        public int? DailyRentalRate { get; set; }
    }
}
