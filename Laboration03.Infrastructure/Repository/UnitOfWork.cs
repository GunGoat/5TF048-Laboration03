using Laboration03.Application.Common.Interfaces;
using Laboration03.Infrastructure.Repository;
using System;
using System.Data.SqlClient;

public class UnitOfWork : IUnitOfWork
{
    private readonly SqlConnection _connection;
    private SqlTransaction _transaction;

    public IActorRepository Actors { get; private set; }
    public IMovieRepository Movies { get; private set; }
    public IDirectorRepository Directors { get; private set; }
    public IGenreRepository Genres { get; private set; }

    // Constructor that accepts a connection string and creates a SqlConnection
    public UnitOfWork(string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        _connection = new SqlConnection(connectionString);
        _connection.Open(); // Open the connection when the UnitOfWork is created
        _transaction = _connection.BeginTransaction(); // Start the transaction

        Actors = new ActorRepository(_connection, _transaction);
        Movies = new MovieRepository(_connection, _transaction);
        Directors = new DirectorRepository(_connection, _transaction);
        Genres = new GenreRepository(_connection, _transaction);
    }

    public void Commit()
    {
        _transaction.Commit();
    }

    public void Rollback()
    {
        _transaction.Rollback();
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _connection?.Dispose();
    }
}
