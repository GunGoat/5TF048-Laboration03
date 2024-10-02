using Laboration03.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Laboration03.Infrastructure.Repository;

/// <summary>
/// Generic repository class that provides common CRUD operations for database entities.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public abstract class Repository<T> : IRepository<T> where T : class
{
    protected readonly SqlConnection _connection;
    protected readonly SqlTransaction _transaction;

    /// <summary>
    /// Gets the name of the table associated with the entity.
    /// </summary>
    protected abstract string TableName { get; }

    /// <summary>
    /// Gets the name of the ID column associated with the entity (e.g., MovieID, ActorID).
    /// </summary>
    protected abstract string IdColumnName { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Repository{T}"/> class.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <param name="transaction">The current database transaction.</param>
    protected Repository(SqlConnection connection, SqlTransaction transaction)
    {
        _connection = connection;
        _transaction = transaction;
    }

    /// <summary>
    /// Retrieves an entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity to retrieve.</param>
    /// <returns>The entity if found; otherwise, <c>null</c>.</returns>
    public virtual T GetById(int id)
    {
        string query = $"SELECT * FROM {TableName} WHERE {IdColumnName} = @id";
        using (SqlCommand command = new SqlCommand(query, _connection, _transaction))
        {
            command.Parameters.AddWithValue("@id", id);
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return MapReaderToEntity(reader);
                }
                return null;
            }
        }
    }

    /// <summary>
    /// Retrieves all entities from the associated table.
    /// </summary>
    /// <returns>A collection of all entities in the table.</returns>
    public virtual IEnumerable<T> GetAll()
    {
        string query = $"SELECT * FROM {TableName}";
        using (SqlCommand command = new SqlCommand(query, _connection, _transaction))
        {
            using (var reader = command.ExecuteReader())
            {
                List<T> entities = new List<T>();
                while (reader.Read())
                {
                    entities.Add(MapReaderToEntity(reader));
                }
                return entities;
            }
        }
    }

    /// <summary>
    /// Adds a new entity to the database and retrieves the generated ID.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>The ID of the newly added entity.</returns>
    public virtual int Add(T entity)
    {
        // The query SCOPE_IDENTITY() is used to retrieve the generated ID
        string query = $"INSERT INTO {TableName} {GetInsertColumns()} VALUES {GetInsertValues()}; SELECT SCOPE_IDENTITY();";
        using (SqlCommand command = new SqlCommand(query, _connection, _transaction))
        {
            SetInsertParameters(command, entity);

            // Execute the command and get the newly inserted ID
            object result = command.ExecuteScalar();
            if (result != null)
            {
                // Convert the result to an integer 
                return Convert.ToInt32(result);
            }

            throw new InvalidOperationException("Failed to retrieve the ID of the newly inserted entity.");
        }
    }


    /// <summary>
    /// Updates an existing entity in the database.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    public virtual void Update(T entity)
    {
        // Use reflection to get the ID property dynamically
        var idProperty = entity.GetType().GetProperty(IdColumnName);
        if (idProperty == null)
        {
            throw new InvalidOperationException($"No property '{IdColumnName}' found on entity '{typeof(T).Name}'.");
        }

        // Get the value of the ID property from the entity
        var idValue = idProperty.GetValue(entity);

        // Construct the SQL query
        string query = $"UPDATE {TableName} SET {GetUpdateSetClause()} WHERE {IdColumnName} = @id";
        using (SqlCommand command = new SqlCommand(query, _connection, _transaction))
        {
            command.Parameters.AddWithValue("@id", idValue);
            SetUpdateParameters(command, entity);
            command.ExecuteNonQuery();
        }
    }

    /// <summary>
    /// Deletes an entity from the database by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity to delete.</param>
    public virtual void Delete(int id)
    {
        string query = $"DELETE FROM {TableName} WHERE {IdColumnName} = @id";
        using (SqlCommand command = new SqlCommand(query, _connection, _transaction))
        {
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
    }

    /// <summary>
    /// Maps a <see cref="SqlDataReader"/> to an entity object.
    /// </summary>
    /// <param name="reader">The data reader containing the result set.</param>
    /// <returns>The mapped entity object.</returns>
    protected abstract T MapReaderToEntity(SqlDataReader reader);

    /// <summary>
    /// Gets a string representing the columns to insert into the table.
    /// </summary>
    /// <returns>A string of column names for the insert statement.</returns>
    protected abstract string GetInsertColumns();

    /// <summary>
    /// Gets a string representing the values to insert into the table.
    /// </summary>
    /// <returns>A string of values for the insert statement.</returns>
    protected abstract string GetInsertValues();

    /// <summary>
    /// Sets the SQL parameters for inserting a new entity.
    /// </summary>
    /// <param name="command">The SQL command to configure.</param>
    /// <param name="entity">The entity to insert.</param>
    protected abstract void SetInsertParameters(SqlCommand command, T entity);

    /// <summary>
    /// Gets a string representing the columns and values to update in the table.
    /// </summary>
    /// <returns>A string for the update set clause.</returns>
    protected abstract string GetUpdateSetClause();

    /// <summary>
    /// Sets the SQL parameters for updating an existing entity.
    /// </summary>
    /// <param name="command">The SQL command to configure.</param>
    /// <param name="entity">The entity to update.</param>
    protected abstract void SetUpdateParameters(SqlCommand command, T entity);
}
