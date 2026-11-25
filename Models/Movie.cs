using System;
using System.Collections.Generic;

namespace MovieApi.Models;

public class Movie
{
  public Guid Id { get; set; }

  public string Title { get; set; } = string.Empty;

  public int Year { get; set; }

  public TimeSpan Duration { get; set; }

  // ska man som användare kunne skapa en Movie och senare lägga till Details?
  // eller ska flödet kräva details i förväg för att acceptera att en Movie läggs in??
  // för tillfället tänker jag att man kan lägga in en Movie utan detaljer.
  // Apiet kan returnera ofärdiga Movies utan MovieDetails och med beroende på område (role=admin vs role=user)
  public MovieDetails? MovieDetails { get; set; }

  public ICollection<Review> Reviews { get; set; } = [];

  // kopplingstabeller
  public ICollection<MovieActor> MovieActors { get; set; } = [];

  public ICollection<MovieGenre> MovieGenres { get; set; } = [];
}