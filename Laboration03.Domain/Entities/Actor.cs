using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration03.Domain.Entities;

public class Actor
{
    public int ActorID { get; set; }
    public required string Name { get; set; }
    public string? Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Nationality { get; set; }
    public string? Biography { get; set; }
    public string? ProfileUrl { get; set; }
}
