using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration03.Domain.Entities;

public class Genre
{
    public int GenreID { get; set; }
    public required string GenreName { get; set; }
}
