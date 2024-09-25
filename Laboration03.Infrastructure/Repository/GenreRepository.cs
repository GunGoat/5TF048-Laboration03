using Laboration03.Application.Common.Interfaces;
using Laboration03.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration03.Infrastructure.Repository;

public class GenreRepository : Repository<Genre>, IGenreRepository
{
    protected override string TableName => "Genres";
    protected override string IdColumnName => "GenreID";

    public GenreRepository(SqlConnection connection, SqlTransaction transaction) : 
        base(connection, transaction)
    {
    }

    protected override Genre MapReaderToEntity(SqlDataReader reader)
    {
        return new Genre
        {
            GenreID = (int)reader["GenreID"],
            GenreName = reader["GenreName"].ToString()!
        };
    }

    protected override string GetInsertColumns()
    {
        return "(GenreName)";
    }

    protected override string GetInsertValues()
    {
        return "(@GenreName)";
    }

    protected override void SetInsertParameters(SqlCommand command, Genre entity)
    {
        command.Parameters.AddWithValue("@GenreName", entity.GenreName);
    }

    protected override string GetUpdateSetClause()
    {
        return "GenreName = @GenreName";
    }

    protected override void SetUpdateParameters(SqlCommand command, Genre entity)
    {
        command.Parameters.AddWithValue("@GenreName", entity.GenreName);
        command.Parameters.AddWithValue("@GenreID", entity.GenreID);
    }
}
