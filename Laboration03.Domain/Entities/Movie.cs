using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laboration03.Domain.Entities;

public class Movie
{
    public int MovieID { get; set; }

    [Required]
    public string Title { get; set; }

    public DateTime? ReleaseDate { get; set; }

    [Range(0, 10, ErrorMessage = "Rating must be between 0 and 10.")]
    public decimal? Rating { get; set; } 

    [Range(1, int.MaxValue, ErrorMessage = "Duration must be a positive number.")]
    public int? Duration { get; set; } // Duration in minutes

    public int? DirectorID { get; set; }

    [NotMapped]
    public IFormFile? Preview { get; set; }

    public string? PreviewUrl { get; set; }

    public string? Description { get; set; }

    // Navigation properties
    public Director? Director { get; set; }
    public IEnumerable<Actor>? Actors { get; set; }
    public IEnumerable<Genre>? Genres { get; set; }
}
