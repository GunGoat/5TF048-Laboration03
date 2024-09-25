using Laboration03.Application.Common.Interfaces;
using Laboration03.Domain.Entities;
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
            // Assuming lazy loading for Director or could load manually if required
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
}

