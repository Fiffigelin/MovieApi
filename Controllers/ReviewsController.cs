
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
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
  public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
  {
    var reviews = await _context.Reviews.ToListAsync();

    return Ok(reviews);
  }

  // GET: api/reviews/{id}
  [HttpGet("{id}")]
  public async Task<ActionResult<Review>> GetReview(Guid id)
  {
    var review = await _context.Reviews
        .FirstOrDefaultAsync(a => a.Id == id);

    if (review == null)
    {
      return NotFound();
    }

    return Ok(review);
  }

  // POST: api/reviews
  [HttpPost]
  public async Task<ActionResult<Review>> CreateReview(Review review)
  {
    review.Id = Guid.NewGuid();

    _context.Reviews.Add(review);
    await _context.SaveChangesAsync();

    return CreatedAtAction(
        nameof(GetReview),
        new { id = review.Id },
        review);
  }

  // PUT: api/reviews/{id}
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateReview(Guid id, Review review)
  {
    if (id != review.Id)
    {
      return BadRequest();
    }

    var existingReview = await _context.Reviews.FindAsync(id);

    if (existingReview == null)
    {
      return NotFound();
    }

    existingReview.ReviewerName = review.ReviewerName;
    existingReview.Comment = review.Comment;
    existingReview.Rating = review.Rating;

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
}
