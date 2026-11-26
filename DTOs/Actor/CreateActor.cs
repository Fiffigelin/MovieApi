
using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs.Actor;

public class CreateActorDto
{
  [Required]
  [StringLength(100, MinimumLength = 2)]
  public required string Name { get; set; }

  [Range(1900, 2100)]
  public int? BirthYear { get; set; }
}