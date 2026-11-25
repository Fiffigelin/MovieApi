using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs.Actor;

public class UpdateMovieActorDto
{
  [Required]
  [StringLength(100, MinimumLength = 1)]
  public required string Role { get; set; }
}