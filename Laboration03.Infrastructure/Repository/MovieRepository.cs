using Laboration03.Application.Common.Interfaces;
using Laboration03.Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration03.Infrastructure.Repository;


public class MovieRepository : Repository<Movie>, IMovieRepository
{
    protected override string TableName => "Movies";
    protected override string IdColumnName => "MovieID";

    public MovieRepository(SqlConnection connection, SqlTransaction transaction) 
        : base(connection, transaction)
    {
    }

    protected override Movie MapReaderToEntity(SqlDataReader reader)
    {
        return new Movie
        {
            MovieID = (int)reader["MovieID"],
            Title = reader["Title"].ToString()!,
            ReleaseDate = reader["ReleaseDate"] as DateTime?,
            Rating = reader["Rating"] as decimal?,
            Duration = reader["Duration"] as int?,
            DirectorID = reader["DirectorID"] as int?,
            PreviewUrl = reader["PreviewUrl"] as string,
            Description = reader["Description"] as string,
        };
    }

    protected override string GetInsertColumns()
    {
        return "(Title, ReleaseDate, Rating, Duration, DirectorID, PreviewUrl, Description)";
    }

    protected override string GetInsertValues()
    {
        return "(@Title, @ReleaseDate, @Rating, @Duration, @DirectorID, @PreviewUrl, @Description)";
    }

    protected override void SetInsertParameters(SqlCommand command, Movie entity)
    {
        command.Parameters.AddWithValue("@Title", entity.Title);
        command.Parameters.AddWithValue("@ReleaseDate", (object?)entity.ReleaseDate ?? DBNull.Value);
        command.Parameters.AddWithValue("@Rating", (object?)entity.Rating ?? DBNull.Value);
        command.Parameters.AddWithValue("@Duration", (object?)entity.Duration ?? DBNull.Value);
        command.Parameters.AddWithValue("@DirectorID", (object?)entity.DirectorID ?? DBNull.Value);
        command.Parameters.AddWithValue("@PreviewUrl", (object?)entity.PreviewUrl ?? DBNull.Value);
        command.Parameters.AddWithValue("@Description", (object?)entity.Description ?? DBNull.Value);
    }

    protected override string GetUpdateSetClause()
    {
        return "Title = @Title, ReleaseDate = @ReleaseDate, Rating = @Rating, Duration = @Duration, DirectorID = @DirectorID, PreviewUrl = @PreviewUrl, Description = @Description";
    }

    protected override void SetUpdateParameters(SqlCommand command, Movie entity)
    {
        command.Parameters.AddWithValue("@Title", entity.Title);
        command.Parameters.AddWithValue("@ReleaseDate", (object?)entity.ReleaseDate ?? DBNull.Value);
        command.Parameters.AddWithValue("@Rating", (object?)entity.Rating ?? DBNull.Value);
        command.Parameters.AddWithValue("@Duration", (object?)entity.Duration ?? DBNull.Value);
        command.Parameters.AddWithValue("@DirectorID", (object?)entity.DirectorID ?? DBNull.Value);
        command.Parameters.AddWithValue("@PreviewUrl", (object?)entity.PreviewUrl ?? DBNull.Value);
        command.Parameters.AddWithValue("@Description", (object?)entity.Description ?? DBNull.Value);
        command.Parameters.AddWithValue("@MovieID", entity.MovieID);
    }

    public IEnumerable<Movie> GetMoviesWithDetails(
        string titleSearch = null,
        string sortColumn = "Title",
        string sortOrder = "ASC",
        params int[] movieIds)
    {
        // Validate sort column and sort order
        var validSortColumns = new List<string> { "Title", "ReleaseDate", "Rating", "Duration" };
        if (!validSortColumns.Contains(sortColumn))
        {
            throw new ArgumentException("Invalid sort column. Valid options are: Title, ReleaseDate, Rating, Duration.");
        }

        if (sortOrder.ToUpper() != "ASC" && sortOrder.ToUpper() != "DESC")
        {
            throw new ArgumentException("Invalid sort order. Valid options are: ASC or DESC.");
        }

        // Base query
        string query = "SELECT * FROM View_MoviesWithDetails";

        // Initialize a list to hold query conditions
        List<string> conditions = new List<string>();

        // If movieIds are provided, add WHERE clause for movie IDs
        if (movieIds != null && movieIds.Length > 0)
        {
            string ids = string.Join(",", movieIds);
            conditions.Add($"MovieID IN ({ids})");
        }

        // If titleSearch is provided, add WHERE clause for title
        if (!string.IsNullOrEmpty(titleSearch))
        {
            conditions.Add("Title LIKE @titleSearch");
        }

        // Combine conditions into the query
        if (conditions.Count > 0)
        {
            query += " WHERE " + string.Join(" AND ", conditions);
        }

        // Add sorting clause
        query += $" ORDER BY {sortColumn} {sortOrder}";

        using (SqlCommand command = new SqlCommand(query, _connection, _transaction))
        {
            // Add the titleSearch parameter to the query, if it's provided
            if (!string.IsNullOrEmpty(titleSearch))
            {
                command.Parameters.AddWithValue("@titleSearch", "%" + titleSearch + "%");
            }

            using (var reader = command.ExecuteReader())
            {
                List<Movie> movies = new List<Movie>();

                while (reader.Read())
                {
                    var movie = new Movie
                    {
                        MovieID = (int)reader["MovieID"],
                        Title = reader["Title"].ToString()!,
                        ReleaseDate = reader["ReleaseDate"] as DateTime?,
                        Rating = reader["Rating"] as decimal?,
                        Duration = reader["Duration"] as int?,
                        PreviewUrl = reader["PreviewUrl"] as string,
                        Description = reader["Description"] as string,
                        DirectorID = reader["DirectorID"] as int?,

                        // Deserialize JSON for actors and genres
                        Director = JsonConvert.DeserializeObject<Director>(reader["DirectorJson"].ToString() ?? "{}"),
                        Actors = JsonConvert.DeserializeObject<IEnumerable<Actor>>(reader["ActorsJson"].ToString() ?? "[]"),
                        Genres = JsonConvert.DeserializeObject<IEnumerable<Genre>>(reader["GenresJson"].ToString() ?? "[]")
                    };

                    movies.Add(movie);
                }

                return movies;
            }
        }
    }



    public void UpdateMovieGenres(int movieId, IEnumerable<int> genreIds)
    {
        using (SqlCommand command = new SqlCommand("UpdateMovieGenres", _connection, _transaction))
        {
            command.CommandType = System.Data.CommandType.StoredProcedure;

            // Set the parameters for the stored procedure
            command.Parameters.AddWithValue("@MovieID", movieId);
            command.Parameters.AddWithValue("@GenreIDs", string.Join(",", genreIds));

            // Execute the stored procedure
            command.ExecuteNonQuery();
        }
    }

    public void UpdateMovieActors(int movieId, IEnumerable<int> actorsIds)
    {
        using (SqlCommand command = new SqlCommand("UpdateMovieActors", _connection, _transaction))
        {
            command.CommandType = System.Data.CommandType.StoredProcedure;

            // Set the parameters for the stored procedure
            command.Parameters.AddWithValue("@MovieID", movieId);
            command.Parameters.AddWithValue("@ActorIDs", string.Join(",", actorsIds));

            // Execute the stored procedure
            command.ExecuteNonQuery();
        }
    }
}