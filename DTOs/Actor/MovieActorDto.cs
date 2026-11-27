using System;
using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs.Actor;

public class MovieActorDto
{
  public Guid Id { get; set; }

  [Required]
  [StringLength(100, MinimumLength = 3)]
  public required string Name { get; set; }

  public int? BirthYear { get; set; }

  [Required]
  [StringLength(100, MinimumLength = 1)]
  public required string Role { get; set; }
}