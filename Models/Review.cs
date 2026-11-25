using System;

namespace MovieApi.Models;

public class Review
{
  public Guid Id { get; set; }

  public string ReviewerName { get; set; } = string.Empty;

  public string Comment { get; set; } = string.Empty;

  public int Rating { get; set; } //begränsning mellan 1-5

  public Guid MovieId { get; set; }

  // M:1 till Movie
  public Movie Movie { get; set; } = null!;
}