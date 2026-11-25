using System;
using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs.Actor;

public class CreateMovieActorDto
{
  public Guid MovieId { get; set; }

  public Guid ActorId { get; set; }

  [Required]
  [StringLength(100, MinimumLength = 1)]
  public required string Role { get; set; }
}