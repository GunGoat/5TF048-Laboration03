﻿using Laboration03.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration03.Application.Common.Interfaces;

public interface IMovieRepository : IRepository<Movie>
{
    public IEnumerable<Movie> GetMoviesWithDetails(params int[] movieIds);
}
