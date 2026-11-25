
using System;
using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs.Genre;

public class GenreDto
{
  public Guid Id { get; set; }

  [Required]
  [StringLength(100, MinimumLength = 1)]
  public required string Name { get; set; }
}