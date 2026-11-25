using System;
using System.Collections.Generic;

namespace MovieApi.Models;

public class Genre
{
  public Guid Id { get; set; }

  public string Name { get; set; } = string.Empty;

  // kopplingstabell
  public ICollection<MovieGenre> MovieGenres { get; set; } = [];
}