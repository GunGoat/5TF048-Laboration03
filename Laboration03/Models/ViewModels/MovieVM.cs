using Laboration03.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laboration03.Models.ViewModels;

public class MovieVM
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

    // Properties for selecting from available actors, genres, and director
    public List<int> SelectedActorIDs { get; set; } = new List<int>(); // For multi-select actors
    public List<int> SelectedGenreIDs { get; set; } = new List<int>(); // For multi-select genres

    // Available options for the user to choose from
    public IEnumerable<Actor>? AvailableActors { get; set; }
    public IEnumerable<Genre>? AvailableGenres { get; set; }
    public IEnumerable<Director>? AvailableDirectors { get; set; }
}
