using System;
using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs.MovieDetail;

public class MovieDetailsDto
{
  public Guid Id { get; set; }

  [Required]
  [StringLength(100, MinimumLength = 3)]
  public required string Synopsis { get; set; }

  [Required]
  [StringLength(1000, MinimumLength = 3)]
  public required string Language { get; set; }

  public int? Budget { get; set; } = 0;
}