using System;

namespace MovieApi.Models;

// kopplingstabell

public class MovieActor
{
  public Guid Id { get; set; }
  public int MovieId { get; set; }

  public int ActorId { get; set; }

  public string Role { get; set; } = string.Empty;

  public Movie Movie { get; set; } = null!;

  public Actor Actor { get; set; } = null!;
}