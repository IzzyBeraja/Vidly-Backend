﻿using System.ComponentModel.DataAnnotations;

namespace VidlyBackend.Dto
{
    public class MovieCreateDto
    {
        // Only available because I would like to add specific movies
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int? NumberInStock { get; set; }

        [Required]
        public int? DailyRentalRate { get; set; }
    }
}
