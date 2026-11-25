using System;

namespace MovieApi.Models;

public class MovieDetails
{
  public Guid Id { get; set; }

  public string Synopsis { get; set; } = string.Empty;

  public string Language { get; set; } = string.Empty;

  public int? Budget { get; set; } = 0;

  // FK till Movie
  public Guid MovieId { get; set; }

  // Direktkoppling till Movie. Kan inte vara null. 1:1
  public Movie Movie { get; set; } = null!;
}