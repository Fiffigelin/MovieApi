
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
public class ActorsController : ControllerBase
{
  private readonly MovieDbContext _context;

  public ActorsController(MovieDbContext context)
  {
    _context = context;
  }

  // GET: api/actors
  [HttpGet]
  public async Task<ActionResult<IEnumerable<Actor>>> GetActors()
  {
    var actors = await _context.Actors.ToListAsync();

    return Ok(actors);
  }

  // GET: api/actors/{id}
  [HttpGet("{id}")]
  public async Task<ActionResult<Actor>> GetActor(Guid id)
  {
    var actor = await _context.Actors
        .FirstOrDefaultAsync(a => a.Id == id);

    if (actor == null)
    {
      return NotFound();
    }

    return Ok(actor);
  }

  // POST: api/actors
  [HttpPost]
  public async Task<ActionResult<Actor>> CreateActor(Actor actor)
  {
    actor.Id = Guid.NewGuid();

    _context.Actors.Add(actor);
    await _context.SaveChangesAsync();

    return CreatedAtAction(
        nameof(GetActor),
        new { id = actor.Id },
        actor);
  }

  // PUT: api/actors/{id}
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateActor(Guid id, Actor actor)
  {
    if (id != actor.Id)
    {
      return BadRequest();
    }

    var existingActor = await _context.Actors.FindAsync(id);

    if (existingActor == null)
    {
      return NotFound();
    }

    existingActor.Name = actor.Name;
    existingActor.BirthYear = actor.BirthYear;

    await _context.SaveChangesAsync();

    return NoContent();
  }

  // DELETE: api/actors/{id}
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteActor(Guid id)
  {
    var actor = await _context.Actors.FindAsync(id);

    if (actor == null)
    {
      return NotFound();
    }

    _context.Actors.Remove(actor);
    await _context.SaveChangesAsync();

    return NoContent();
  }
}
