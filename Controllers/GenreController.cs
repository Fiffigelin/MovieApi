
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
public class GenreController : ControllerBase
{
  private readonly MovieDbContext _context;

  public GenreController(MovieDbContext context)
  {
    _context = context;
  }

  // GET: api/genre
  [HttpGet]
  public async Task<ActionResult<IEnumerable<Genre>>> GetAllGenre()
  {
    var allGenre = await _context.Genre.ToListAsync();

    return Ok(allGenre);
  }

  // GET: api/genre/{id}
  [HttpGet("{id}")]
  public async Task<ActionResult<Genre>> GetGenre(Guid id)
  {
    var genre = await _context.Genre
        .FirstOrDefaultAsync(a => a.Id == id);

    if (genre == null)
    {
      return NotFound();
    }

    return Ok(genre);
  }

  // POST: api/genre
  [HttpPost]
  public async Task<ActionResult<Genre>> CreateGenre(Genre genre)
  {
    genre.Id = Guid.NewGuid();

    _context.Genre.Add(genre);
    await _context.SaveChangesAsync();

    return CreatedAtAction(
        nameof(GetGenre),
        new { id = genre.Id },
        genre);
  }

  // PUT: api/genre/{id}
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateGenre(Guid id, Genre genre)
  {
    if (id != genre.Id)
    {
      return BadRequest();
    }

    var existingGenre = await _context.Genre.FindAsync(id);

    if (existingGenre == null)
    {
      return NotFound();
    }

    existingGenre.Name = genre.Name;

    await _context.SaveChangesAsync();

    return NoContent();
  }

  // DELETE: api/genre/{id}
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteGenre(Guid id)
  {
    var genre = await _context.Genre.FindAsync(id);

    if (genre == null)
    {
      return NotFound();
    }

    _context.Genre.Remove(genre);
    await _context.SaveChangesAsync();

    return NoContent();
  }
}
