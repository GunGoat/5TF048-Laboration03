using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration03.Domain.Entities;

public class Director
{
    public int DirectorID { get; set; }
    public required string Name { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Nationality { get; set; }
    public string? Biography { get; set; }
}
