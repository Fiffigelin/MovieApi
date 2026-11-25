
namespace MovieApi.DTOs.Review;

public class UpdateReviewDto
{
  public required string ReviewerName { get; set; }

  public required string Comment { get; set; }

  [ValidRating]
  public int Rating { get; set; }
}