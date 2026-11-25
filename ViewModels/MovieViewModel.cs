using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MovieApi.DTOs.Genre;
using MovieApi.DTOs.Movie;

namespace MovieApi.ViewModels;

public class MovieViewModel
{
  [Required]
  public required MovieDto Movie { get; set; }

  [Required]
  public required MovieDetailViewModel Details { get; set; }

  [Required]
  public required IEnumerable<GenreDto> Genres { get; set; } = [];

  [Required]
  public required IEnumerable<MovieActorViewModel> Actors { get; set; } = [];

  public required IEnumerable<ReviewViewModel> Reviews { get; set; } = [];
}