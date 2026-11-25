
using System;

namespace MovieApi.DTOs.Genre;

public class MovieGenreDto
{
  public Guid MovieId { get; set; }

  public Guid GenreId { get; set; }
}