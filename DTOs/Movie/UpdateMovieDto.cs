using System;
using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs.Movie;

public class UpdateMovieDto
{
  [Required]
  [StringLength(200, MinimumLength = 1)]
  public string Title { get; set; } = string.Empty;

  [Required]
  [Range(1900, 2100)]
  public int Year { get; set; }

  [Required]
  [ValidMovieDuration]
  public TimeSpan Duration { get; set; }
}