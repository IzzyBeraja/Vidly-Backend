using System.ComponentModel.DataAnnotations;
using VidlyBackend.Models;

namespace VidlyBackend.Dto
{
    public class UserAuthDto
    {
        [Required]
        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$",
        ErrorMessage = "The Email field is not a valid e-mail address.")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
