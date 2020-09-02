using System.ComponentModel.DataAnnotations;
using VidlyBackend.Models;

namespace VidlyBackend.Dto
{
    public class UserCreateDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
