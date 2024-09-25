using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration03.Application.Common.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IActorRepository Actors { get; }
    IMovieRepository Movies { get; }
    IDirectorRepository Directors { get; }
    IGenreRepository Genres { get; }

    void Commit();
    void Rollback();
}