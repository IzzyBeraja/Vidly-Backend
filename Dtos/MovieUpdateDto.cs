using System.ComponentModel.DataAnnotations;
using VidlyBackend.Models;

namespace VidlyBackend.Dto
{
    public class MovieUpdateDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public int? NumberInStock { get; set; }

        [Required]
        public int? DailyRentalRate { get; set; }

        [Required]
        public Genre genre { get; set; }
    }
}
