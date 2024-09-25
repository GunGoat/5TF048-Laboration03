using Laboration03.Application.Common.Interfaces;
using Laboration03.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration03.Infrastructure.Repository;

public class DirectorRepository : Repository<Director>, IDirectorRepository
{
    protected override string TableName => "Directors";
    protected override string IdColumnName => "DirectorID";

    public DirectorRepository(SqlConnection connection, SqlTransaction transaction) : 
        base(connection, transaction)
    {
    }

    protected override Director MapReaderToEntity(SqlDataReader reader)
    {
        return new Director
        {
            DirectorID = (int)reader["DirectorID"],
            Name = reader["Name"].ToString()!,
            DateOfBirth = reader["DateOfBirth"] as DateTime?,
            Nationality = reader["Nationality"] as string,
            Biography = reader["Biography"] as string
        };
    }

    protected override string GetInsertColumns()
    {
        return "(Name, DateOfBirth, Nationality, Biography)";
    }

    protected override string GetInsertValues()
    {
        return "(@Name, @DateOfBirth, @Nationality, @Biography)";
    }

    protected override void SetInsertParameters(SqlCommand command, Director entity)
    {
        command.Parameters.AddWithValue("@Name", entity.Name);
        command.Parameters.AddWithValue("@DateOfBirth", (object?)entity.DateOfBirth ?? DBNull.Value);
        command.Parameters.AddWithValue("@Nationality", (object?)entity.Nationality ?? DBNull.Value);
        command.Parameters.AddWithValue("@Biography", (object?)entity.Biography ?? DBNull.Value);
    }

    protected override string GetUpdateSetClause()
    {
        return "Name = @Name, DateOfBirth = @DateOfBirth, Nationality = @Nationality, Biography = @Biography";
    }

    protected override void SetUpdateParameters(SqlCommand command, Director entity)
    {
        command.Parameters.AddWithValue("@Name", entity.Name);
        command.Parameters.AddWithValue("@DateOfBirth", (object?)entity.DateOfBirth ?? DBNull.Value);
        command.Parameters.AddWithValue("@Nationality", (object?)entity.Nationality ?? DBNull.Value);
        command.Parameters.AddWithValue("@Biography", (object?)entity.Biography ?? DBNull.Value);
        command.Parameters.AddWithValue("@DirectorID", entity.DirectorID);
    }
}
