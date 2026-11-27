
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

  /// <summary>
  /// Hämtar alla recensioner som finns registrerade.
  /// </summary>
  /// <returns>En lista med samtliga recensioner.</returns>
  [HttpGet]
  public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviews()
  {
    var data = await _context.Reviews.ToListAsync();

    var result = data.Select(MapEntityToDto).ToList();

    return Ok(result);
  }

  /// <summary>
  /// Hämtar en recension baserat på dess ID.
  /// </summary>
  /// <param name="id">ID för recensionen som ska hämtas.</param>
  /// <returns>Recensionens namn, kommentar och betyg.</returns>
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

  /// <summary>
  /// Hämtar alla recensioner som tillhör en specifik film.
  /// </summary>
  /// <param name="id">ID för filmen vars recensioner ska hämtas.</param>
  /// <returns>En lista med filmens recensioner.</returns>
  [HttpGet("~/api/movies/{id}/reviews")]
  public async Task<ActionResult<ReviewDto>> GetReviewByMovie(Guid id)
  {
    var data = await _context.Reviews.Where(r => r.MovieId == id).ToListAsync();

    var result = data.Select(MapEntityToDto).ToList();

    return Ok(result);
  }

  /// <summary>
  /// Skapar en ny recension för en befintlig film.
  /// </summary>
  /// <param name="id">ID för filmen som recensionen ska kopplas till.</param>
  /// <param name="dto">Recensentens namn, kommentar och betyg.</param>
  /// <returns>Den skapade recensionen.</returns>
  [HttpPost("~/api/movies/{id}/reviews")]
  public async Task<ActionResult<ReviewDto>> CreateReview(Guid id, CreateReviewDto dto)
  {
    var movieExists = await _context.Movies
    .AnyAsync(m => m.Id == id);

    if (!movieExists)
    {
      return NotFound();
    }

    var data = MapDtoToEntity(id, dto);

    _context.Reviews.Add(data);
    await _context.SaveChangesAsync();

    var result = MapEntityToDto(data);

    return CreatedAtAction(
        nameof(GetReview),
        new { id = data.Id },
        result);
  }

  /// <summary>
  /// Uppdaterar en befintlig recension.
  /// </summary>
  /// <param name="id">ID för recensionen som ska uppdateras.</param>
  /// <param name="dto">Recensionens nya namn, kommentar och betyg.</param>
  /// <returns>204 om uppdateringen lyckades, annars 404 om recensionen inte hittades.</returns>
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

  /// <summary>
  /// Tar bort en recension baserat på dess ID.
  /// </summary>
  /// <param name="id">ID för recensionen som ska tas bort.</param>
  /// <returns>204 om recensionen togs bort, annars 404 om den inte hittades.</returns>
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
