using System;
using System.Collections.Generic;

namespace MovieApi.Models;

public class Actor
{
  public Guid Id { get; set; }

  public string Name { get; set; } = string.Empty;

  public int? BirthYear { get; set; }

  // kopplingstabell
  public ICollection<MovieActor> MovieActors { get; set; } = [];
}