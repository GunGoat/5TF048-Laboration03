using Laboration03.Application.Common.Interfaces;
using Laboration03.Domain.Entities;
using System;
using System.Data.SqlClient;

namespace Laboration03.Infrastructure.Repository
{
    public class ActorRepository : Repository<Actor>, IActorRepository
    {
        protected override string TableName => "Actors";
        protected override string IdColumnName => "ActorID";

        public ActorRepository(SqlConnection connection, SqlTransaction transaction) :
            base(connection, transaction)
        {
        }

        protected override Actor MapReaderToEntity(SqlDataReader reader)
        {
            return new Actor
            {
                ActorID = (int)reader["ActorID"],
                Name = reader["Name"].ToString()!,
                Gender = reader["Gender"] as string,
                DateOfBirth = reader["DateOfBirth"] as DateTime?,
                Nationality = reader["Nationality"] as string,
                Biography = reader["Biography"] as string,
                ProfileUrl = reader["ProfileUrl"] as string
            };
        }

        protected override string GetInsertColumns()
        {
            return "(Name, Gender, DateOfBirth, Nationality, Biography, ProfileUrl)";
        }

        protected override string GetInsertValues()
        {
            return "(@Name, @Gender, @DateOfBirth, @Nationality, @Biography, @ProfileUrl)";
        }

        protected override void SetInsertParameters(SqlCommand command, Actor entity)
        {
            command.Parameters.AddWithValue("@Name", entity.Name);
            command.Parameters.AddWithValue("@Gender", (object?)entity.Gender ?? DBNull.Value);
            command.Parameters.AddWithValue("@DateOfBirth", (object?)entity.DateOfBirth ?? DBNull.Value);
            command.Parameters.AddWithValue("@Nationality", (object?)entity.Nationality ?? DBNull.Value);
            command.Parameters.AddWithValue("@Biography", (object?)entity.Biography ?? DBNull.Value);
            command.Parameters.AddWithValue("@ProfileUrl", (object?)entity.ProfileUrl ?? DBNull.Value);
        }

        protected override string GetUpdateSetClause()
        {
            return "Name = @Name, Gender = @Gender, DateOfBirth = @DateOfBirth, Nationality = @Nationality, Biography = @Biography, ProfileUrl = @ProfileUrl";
        }

        protected override void SetUpdateParameters(SqlCommand command, Actor entity)
        {
            command.Parameters.AddWithValue("@Name", entity.Name);
            command.Parameters.AddWithValue("@Gender", (object?)entity.Gender ?? DBNull.Value);
            command.Parameters.AddWithValue("@DateOfBirth", (object?)entity.DateOfBirth ?? DBNull.Value);
            command.Parameters.AddWithValue("@Nationality", (object?)entity.Nationality ?? DBNull.Value);
            command.Parameters.AddWithValue("@Biography", (object?)entity.Biography ?? DBNull.Value);
            command.Parameters.AddWithValue("@ProfileUrl", (object?)entity.ProfileUrl ?? DBNull.Value);
            command.Parameters.AddWithValue("@ActorID", entity.ActorID);
        }
    }
}
