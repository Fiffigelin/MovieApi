
using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs.Genre;

public class UpdateGenreDto
{
  [Required]
  [StringLength(100, MinimumLength = 1)]
  public required string Name { get; set; }
}