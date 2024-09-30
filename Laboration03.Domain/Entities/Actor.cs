using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laboration03.Domain.Entities;

public class Actor
{
    public int ActorID { get; set; }

    [Required]
    public string Name { get; set; }

    [RegularExpression(@"^(Male|Female|Other)$", ErrorMessage = "Gender must be 'Male', 'Female', or 'Other'.")]
    public string? Gender { get; set; }

    public DateTime? DateOfBirth { get; set; }
    public string? Nationality { get; set; }
    public string? Biography { get; set; }

    [NotMapped]
    public IFormFile? Profile { get; set; }
    public string? ProfileUrl { get; set; }
}
