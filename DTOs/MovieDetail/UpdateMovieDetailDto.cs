using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs.MovieDetail;

public class UpdateMovieDetailsDto
{
  [StringLength(100, MinimumLength = 3)]
  public required string Synopsis { get; set; }

  [StringLength(100, MinimumLength = 3)]
  public required string Language { get; set; }

  public int? Budget { get; set; } = 0;
}