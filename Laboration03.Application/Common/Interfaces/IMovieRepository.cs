using Laboration03.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration03.Application.Common.Interfaces;

public interface IMovieRepository : IRepository<Movie>
{
    public IEnumerable<Movie> GetMoviesWithDetails(
        string titleSearch = null,
        string sortColumn = "Title",
        string sortOrder = "ASC",
        params int[] movieIds);
    public void UpdateMovieGenres(int movieId, IEnumerable<int> genreIds);
    public void UpdateMovieActors(int movieId, IEnumerable<int> genreIds);
}
