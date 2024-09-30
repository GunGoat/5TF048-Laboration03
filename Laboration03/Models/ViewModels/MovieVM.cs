using Laboration03.Domain.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Laboration03.Models.ViewModels
{
    public class MovieVM
    {
        public Movie Movie { get; set; }

        // List of available actors to choose from
        [Display(Name = "Actors")]
        public IEnumerable<Actor> AvailableActors { get; set; }

        // List of selected actor IDs (for form binding)
        [Display(Name = "Selected Actors")]
        public IEnumerable<int> SelectedActorIds { get; set; }

        // List of available genres to choose from
        [Display(Name = "Genres")]
        public IEnumerable<Genre> AvailableGenres { get; set; }

        // List of selected genre IDs (for form binding)
        [Display(Name = "Selected Genres")]
        public IEnumerable<int> SelectedGenreIds { get; set; }
    }
}
