using System;
using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs.Movie;

public class MovieDto
{
  public Guid Id { get; set; }

  [Required]
  [StringLength(100, MinimumLength = 1)]
  public required string Title { get; set; }

  [Required]
  [Range(1900, 2100)]
  public required int Year { get; set; }

  [Required]
  [ValidMovieDuration]
  public required TimeSpan Duration { get; set; }
}