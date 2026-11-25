using System.ComponentModel.DataAnnotations;

namespace MovieApi.ViewModels;

public class MovieDetailViewModel
{
  [Required]
  [StringLength(100, MinimumLength = 3)]
  public required string Synopsis { get; set; }

  [Required]
  [StringLength(100, MinimumLength = 3)]
  public required string Language { get; set; }

  public int? Budget { get; set; } = 0;
}