using System;

namespace MovieApi.DTOs.Review;

public class CreateReviewDto
{
  public required string ReviewerName { get; set; }

  public required string Comment { get; set; }

  [ValidRating]
  public int Rating { get; set; }

  public required Guid MovieId { get; set; }
}