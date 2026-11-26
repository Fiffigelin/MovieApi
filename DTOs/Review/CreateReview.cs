using System;
using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs.Review;

public class CreateReviewDto
{
  public required string ReviewerName { get; set; }

  public required string Comment { get; set; }

  [Range(1, 5)]
  public int Rating { get; set; }

  public required Guid MovieId { get; set; }
}