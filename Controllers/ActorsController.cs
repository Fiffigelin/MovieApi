
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

  /// <summary>
  /// Hämtar alla skådespelare.
  /// </summary>
  /// <returns>En lista med samtliga skådespelare.</returns>
  [HttpGet]
  public async Task<ActionResult<IEnumerable<ActorDto>>> GetActors()
  {
    var data = await _context.Actors.ToListAsync();

    var result = data
        .Select(MapEntityToDto)
        .ToList();

    return Ok(result);
  }

  /// <summary>
  /// Hämtar en skådespelare baserat på dess ID.
  /// </summary>
  /// <param name="id">ID för skådespelaren som ska hämtas.</param>
  /// <returns>Skådespelarens namn och födelseår.</returns>
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

  /// <summary>
  /// Skapar en ny skådespelare.
  /// </summary>
  /// <param name="dto">Skådespelarens namn och födelseår.</param>
  /// <returns>Den skapade skådespelaren.</returns>
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

  /// <summary>
  /// Uppdaterar informationen för en befintlig skådespelare.
  /// </summary>
  /// <param name="id">ID för skådespelaren som ska uppdateras.</param>
  /// <param name="dto">Skådespelarens nya namn och födelseår.</param>
  /// <returns>204 om uppdateringen lyckades, annars 404 om skådespelaren inte hittades.</returns>
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateActor(Guid id, UpdateActorDto dto)
  {

    var data = await _context.Actors.FindAsync(id);

    if (data == null)
    {
      return NotFound();
    }

    data.Name = dto.Name;
    data.BirthYear = dto.BirthYear;

    await _context.SaveChangesAsync();

    return NoContent();
  }

  /// <summary>
  /// Tar bort en skådespelare baserat på dess ID.
  /// </summary>
  /// <param name="id">ID för skådespelaren som ska tas bort.</param>
  /// <returns>204 om skådespelaren togs bort, annars 404 om den inte hittades.</returns>
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

  /// <summary>
  /// Hämtar alla skådespelare som är kopplade till en specifik film,
  /// inklusive deras roller i filmen.
  /// </summary>
  /// <param name="id">ID för filmen vars skådespelare ska hämtas.</param>
  /// <returns>En lista med filmens skådespelare och deras roller.</returns>
  [HttpGet("~/api/movies/{id}/actors")]
  public async Task<ActionResult<IEnumerable<ActorDto>>> GetActorsByMovie(Guid id)
  {
    var data = await _context.MovieActors.Where(a => a.MovieId == id).Include(a => a.Actor).ToListAsync();

    var result = data.Select(MapActorWithRole).ToList();

    return Ok(result);
  }

  /// <summary>
  /// Kopplar en befintlig skådespelare till en film och anger skådespelarens roll.
  /// </summary>
  /// <param name="movieId">ID för filmen som skådespelaren ska kopplas till.</param>
  /// <param name="actorId">ID för skådespelaren som ska läggas till.</param>
  /// <param name="dto">Information om skådespelarens roll i filmen.</param>
  /// <returns>Den skapade kopplingen mellan filmen och skådespelaren.</returns>
  [HttpPost("~/api/movies/{movieId}/actors/{actorId}")]
  public async Task<ActionResult<MovieActorDto>> AddActorToMovie(Guid movieId, Guid actorId, CreateMovieActorDto dto)
  {
    var actor = await _context.Actors.FirstOrDefaultAsync(a => a.Id == actorId);

    if (actor is null)
    {
      return NotFound();
    }

    var movieExists = await _context.Movies.AnyAsync(m => m.Id == movieId);

    if (!movieExists)
    {
      return NotFound("Could not add actor if movie doesn't exists.");
    }

    var data = MapMovieActorEntity(movieId, actorId, dto);

    _context.MovieActors.Add(data);
    await _context.SaveChangesAsync();

    var result = MapActorWithRole(data);

    return CreatedAtAction(
    nameof(GetMovieActorById),
    new { id = result.Id },
    result);
  }

  [HttpGet("~/api/movie-actors/{id}")]
  public async Task<ActionResult<MovieActorDto>> GetMovieActorById(Guid id)
  {
    var data = await _context.MovieActors
        .Include(ma => ma.Actor)
        .FirstOrDefaultAsync(ma => ma.Id == id);

    if (data is null)
    {
      return NotFound();
    }

    var result = new MovieActorDto()
    {
      Id = data.Id,
      Name = data.Actor.Name,
      Role = data.Role
    };

    return Ok(result);
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

  private static MovieActorDto MapActorWithRole(MovieActor movieActor)
  {
    return new MovieActorDto()
    {
      Id = movieActor.Id,
      Name = movieActor.Actor.Name,
      BirthYear = movieActor.Actor.BirthYear,
      Role = movieActor.Role
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
