using System;
using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs.Actor;

public class CreateMovieActorDto
{
  [Required]
  [StringLength(100, MinimumLength = 1)]
  public required string Role { get; set; }
}