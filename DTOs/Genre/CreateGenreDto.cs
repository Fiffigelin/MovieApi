
using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs.Genre;

public class CreateGenreDto
{
  [Required]
  [StringLength(100, MinimumLength = 1)]
  public required string Name { get; set; }
}