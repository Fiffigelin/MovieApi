using System;
using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs.Review;

public class ReviewDto
{
  public Guid Id { get; set; }

  public required string ReviewerName { get; set; }

  public required string Comment { get; set; }

  [Range(1, 3)]
  public int Rating { get; set; }
}