using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration03.Domain.Entities;

public class Movie
{
    public int MovieID { get; set; }
    public required string Title { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public decimal? Rating { get; set; } // Range between 0 and 10 as per the constraint
    public int? Duration { get; set; } // Duration in minutes
    public int? DirectorID { get; set; } // Foreign key to the Director table
    public string? PreviewUrl { get; set; }
    public string? Description { get; set; }

    // Navigation properties
    public Director? Director { get; set; }
    public IEnumerable<Actor>? Actors { get; set; }
    public IEnumerable<Genre>? Genres { get; set; }
}

