using System;

namespace MovieApi.Models;

// kopplingstabell
public class MovieActor
{
  public Guid Id { get; set; }

  public Guid MovieId { get; set; }

  public Guid ActorId { get; set; }

  public string Role { get; set; } = string.Empty;

  public Movie Movie { get; set; } = null!;

  public Actor Actor { get; set; } = null!;
}