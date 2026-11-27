
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.DTOs.Review;
using MovieApi.Models;

namespace MovieApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewsController : ControllerBase
{
  private readonly MovieDbContext _context;

  public ReviewsController(MovieDbContext context)
  {
    _context = context;
  }

  // GET: api/reviews
  [HttpGet]
  public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviews()
  {
    var data = await _context.Reviews.ToListAsync();

    var result = data.Select(MapEntityToDto).ToList();

    return Ok(result);
  }

  // GET: api/reviews/{id}
  [HttpGet("{id}")]
  public async Task<ActionResult<ReviewDto>> GetReview(Guid id)
  {
    var data = await _context.Reviews
        .FirstOrDefaultAsync(a => a.Id == id);

    if (data == null)
    {
      return NotFound();
    }

    return Ok(MapEntityToDto(data));
  }

  // GET: api/movies/{id}/reviews
  [HttpGet("~/api/movies/{id}/reviews")]
  public async Task<ActionResult<ReviewDto>> GetReviewByMovie(Guid id)
  {
    var data = await _context.Reviews.Where(r => r.MovieId == id).ToListAsync();

    if (data == null)
    {
      return NotFound();
    }

    var result = data.Select(MapEntityToDto).ToList();

    return Ok(result);
  }

  // POST: api/movies/{id]/reviews
  [HttpPost("~/api/movies/{id}/reviews")]
  public async Task<ActionResult<ReviewDto>> CreateReview(Guid id, CreateReviewDto dto)
  {
    var data = MapDtoToEntity(id, dto);

    _context.Reviews.Add(data);
    await _context.SaveChangesAsync();

    return CreatedAtAction(
        nameof(GetReview),
        new { id = data.Id },
        data);
  }

  // PUT: api/reviews/{id}
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateReview(Guid id, UpdateReviewDto dto)
  {

    var data = await _context.Reviews.FindAsync(id);

    if (data == null)
    {
      return NotFound();
    }

    if (id != data.Id)
    {
      return BadRequest();
    }

    data.ReviewerName = dto.ReviewerName;
    data.Comment = dto.Comment;
    data.Rating = dto.Rating;

    await _context.SaveChangesAsync();

    return NoContent();
  }

  // DELETE: api/reviews/{id}
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteReview(Guid id)
  {
    var review = await _context.Reviews.FindAsync(id);

    if (review == null)
    {
      return NotFound();
    }

    _context.Reviews.Remove(review);
    await _context.SaveChangesAsync();

    return NoContent();
  }

  private static Review MapDtoToEntity(Guid movieId, CreateReviewDto dto)
  {
    return new Review()
    {
      Id = Guid.NewGuid(),
      ReviewerName = dto.ReviewerName,
      Comment = dto.Comment,
      Rating = dto.Rating,
      MovieId = movieId
    };
  }

  private static ReviewDto MapEntityToDto(Review data)
  {
    return new ReviewDto()
    {
      Id = data.Id,
      ReviewerName = data.ReviewerName,
      Comment = data.Comment,
      Rating = data.Rating,
    };
  }
}
