using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MovieApi.DTOs.Actor;
using MovieApi.DTOs.Genre;
using MovieApi.DTOs.MovieDetail;
using MovieApi.DTOs.Review;

namespace MovieApi.ViewModels;

public class MovieViewModel
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

  [Required]
  public required MovieDetailsDto Details { get; set; }

  [Required]
  public required IEnumerable<string> Genres { get; set; } = [];

  [Required]
  public required IEnumerable<MovieActorDto> Actors { get; set; } = [];

  [Required]
  public required IEnumerable<ReviewDto> Reviews { get; set; } = [];
}