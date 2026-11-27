
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.DTOs.Actor;
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
  public async Task<ActionResult<IEnumerable<ActorDto>>> GetActors()
  {
    var data = await _context.Actors.ToListAsync();

    var result = data
        .Select(MapEntityToDto)
        .ToList();

    return Ok(result);
  }

  // GET: api/actors/{id}
  [HttpGet("{id}")]
  public async Task<ActionResult<ActorDto>> GetActor(Guid id)
  {
    var data = await _context.Actors
        .FirstOrDefaultAsync(a => a.Id == id);

    if (data == null)
    {
      return NotFound();
    }

    return Ok(MapEntityToDto(data));
  }

  // POST: api/actors
  [HttpPost]
  public async Task<ActionResult<ActorDto>> CreateActor(CreateActorDto dto)
  {
    var data = MapDtoToEntity(dto);

    _context.Actors.Add(data);
    await _context.SaveChangesAsync();

    var result = MapEntityToDto(data);

    return CreatedAtAction(
        nameof(GetActor),
        new { id = result.Id },
        result);
  }

  // PUT: api/actors/{id}
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateActor(Guid id, UpdateActorDto dto)
  {

    var data = await _context.Actors.FindAsync(id);

    if (data == null)
    {
      return NotFound();
    }

    if (id != data.Id)
    {
      return BadRequest();
    }

    data.Name = dto.Name;
    data.BirthYear = dto.BirthYear;

    await _context.SaveChangesAsync();

    return NoContent();
  }

  // DELETE: api/actors/{id}
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteActor(Guid id)
  {
    var data = await _context.Actors.FindAsync(id);

    if (data == null)
    {
      return NotFound();
    }

    _context.Actors.Remove(data);
    await _context.SaveChangesAsync();

    return NoContent();
  }

  [HttpGet("~/api/movies/{id}/actors")]
  public async Task<ActionResult<ActorDto>> GetActorsByMovie(Guid id)
  {
    var data = await _context.MovieActors.Where(a => a.MovieId == id).Include(a => a.Actor).ToListAsync();

    if (data is null)
    {
      return NotFound();
    }

    var result = data.Select(a => MapActorWithRole(a.Role, a.Actor)).ToList();

    return Ok(result);
  }

  [HttpPost("~/api/movies/{movieId}/actors/{actorId}")]
  public async Task<ActionResult<MovieActor>> AddActorToMovie(Guid movieId, Guid actorId, CreateMovieActorDto dto)
  {
    var actor = await _context.Actors.FirstOrDefaultAsync(a => a.Id == actorId);

    if (actor is null)
    {
      return NotFound();
    }

    var data = MapMovieActorEntity(movieId, actorId, dto);

    _context.MovieActors.Add(data);
    await _context.SaveChangesAsync();

    var result = MapActorWithRole(data.Role, data.Actor);

    return CreatedAtAction(
    nameof(GetActor),
    new { id = result.Id },
    result);
  }

  private static Actor MapDtoToEntity(CreateActorDto dto)
  {
    return new Actor()
    {
      Id = Guid.NewGuid(),
      Name = dto.Name,
      BirthYear = dto.BirthYear
    };
  }

  private static ActorDto MapEntityToDto(Actor actor)
  {
    return new ActorDto()
    {
      Id = actor.Id,
      Name = actor.Name,
      BirthYear = actor.BirthYear
    };
  }

  private static MovieActorDto MapActorWithRole(string role, Actor actor)
  {
    return new MovieActorDto()
    {
      Id = actor.Id,
      Name = actor.Name,
      BirthYear = actor.BirthYear,
      Role = role
    };
  }

  private static MovieActor MapMovieActorEntity(Guid movieId, Guid actorId, CreateMovieActorDto dto)
  {
    return new MovieActor()
    {
      Id = Guid.NewGuid(),
      MovieId = movieId,
      ActorId = actorId,
      Role = dto.Role
    };
  }
}
