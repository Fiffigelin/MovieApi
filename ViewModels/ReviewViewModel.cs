using System;

namespace MovieApi.ViewModels;

public class ReviewViewModel
{
  public Guid Id { get; set; }

  public required string ReviewerName { get; set; }

  public required string Comment { get; set; }

  [ValidRating]
  public int Rating { get; set; }
}