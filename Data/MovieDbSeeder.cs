using System;
using System.Collections.Generic;
using System.Linq;
using MovieApi.Models;

namespace MovieApi.Data;

public static class MovieDbSeeder
{
  public static void Seed(MovieDbContext context)
  {
    if (context.Movies.Any())
      return;

    var movies = new List<Movie>
    {
      new() { Id = new Guid("261bb26e-3ddc-426a-8f2b-fbf8d1fd9718"), Title = "The Shawshank Redemption", Year = 1994, Duration = new TimeSpan(1, 30, 0) },
      new() { Id = new Guid("7f91d565-521e-4ffd-a640-f7b52bf2c4cb"), Title = "The Godfather", Year = 1972, Duration = new TimeSpan(1, 30, 0) },
      new() { Id = new Guid("2618aba7-9333-4e57-8ab2-334f6029ad97"), Title = "The Dark Knight", Year = 2008, Duration = new TimeSpan(1, 30, 0) },
      new() { Id = new Guid("a7b93843-4023-4165-a1fd-733b23c04c13"), Title = "Pulp Fiction", Year = 1994, Duration = new TimeSpan(1, 30, 0) },
      new() { Id = new Guid("2e51aa7e-9a7a-46a4-b859-da7cbef6f370"), Title = "Forrest Gump", Year = 1994, Duration = new TimeSpan(1, 30, 0) },
      new() { Id = new Guid("7297e5a2-60a6-423c-9550-144cd3b378a1"), Title = "Inception", Year = 2010, Duration = new TimeSpan(1, 30, 0) },
      new() { Id = new Guid("8da7a5f1-fe0d-44c2-a8a9-c0ec554b3b83"), Title = "The Matrix", Year = 1999, Duration = new TimeSpan(1, 30, 0) },
      new() { Id = new Guid("3525e0fb-a297-4fd8-acb4-0ab1a7ef00e0"), Title = "Interstellar", Year = 2014, Duration = new TimeSpan(1, 30, 0) },
      new() { Id = new Guid("0500a043-71d6-4fc6-a819-08208b326db6"), Title = "Parasite", Year = 2019, Duration = new TimeSpan(1, 30, 0) },
      new() { Id = new Guid("db64cc44-02fb-4e55-8c77-a117c15dbea7"), Title = "Spirited Away", Year = 2001, Duration = new TimeSpan(1, 30, 0) }
    };

    var actors = new List<Actor>
    {
      new() { Id = new Guid("f4008e34-6dd8-41bf-a454-27f4c063f12b"), Name = "Tim Robbins", BirthYear = 1958 },
      new() { Id = new Guid("39f9388d-1dd3-4f66-8883-9ca94902d839"), Name = "Morgan Freeman", BirthYear = 1937 },
      new() { Id = new Guid("ca52092b-dd2b-4426-9151-e141c7d3d17a"), Name = "Marlon Brando", BirthYear = 1924 },
      new() { Id = new Guid("dba2a269-f2ff-400a-9586-cacf03266750"), Name = "Al Pacino", BirthYear = 1940 },
      new() { Id = new Guid("af65590f-fdf1-4010-afee-67c2d0cf93d6"), Name = "Christian Bale", BirthYear = 1974 },
      new() { Id = new Guid("d16be9e7-639a-43fe-b811-cc0d8d8c9195"), Name = "Heath Ledger", BirthYear = 1979 },
      new() { Id = new Guid("1b91ea44-015b-4e01-ae37-6c75847d620b"), Name = "John Travolta", BirthYear = 1954 },
      new() { Id = new Guid("ade5edea-c0bf-42ef-9fba-b6b71c5b4551"), Name = "Tom Hanks", BirthYear = 1956 },
      new() { Id = new Guid("c8f5eed4-385e-4aae-bf86-f9a92e2e3f95"), Name = "Leonardo DiCaprio", BirthYear = 1974 },
      new() { Id = new Guid("af076ba2-12c3-46c3-aab2-5b55bbfa6880"), Name = "Keanu Reeves", BirthYear = 1964 }
    };

    var genres = new List<Genre>
    {
      new() { Id = new Guid("8f68b2b1-dec2-4b33-86b7-969affad5ef2"), Name = "Drama" },
      new() { Id = new Guid("b8b6ed01-3200-44f6-b663-eef936328e76"), Name = "Crime" },
      new() { Id = new Guid("7e085268-ae56-436f-baaf-5f496e084cd1"), Name = "Action" },
      new() { Id = new Guid("cd868400-c90a-423f-ac3d-659e5bfd4173"), Name = "Sci-Fi" },
      new() { Id = new Guid("5ea1a68f-a643-44b8-9848-dc40eaf539ff"), Name = "Animation" }
    };

    var details = new List<MovieDetails>
    {
      new() { Id = Guid.Parse("ae7b7d70-b6c2-4f94-ab4a-35aa7b9caebd"), MovieId = Guid.Parse("261bb26e-3ddc-426a-8f2b-fbf8d1fd9718"), Synopsis = "Two inmates form a friendship over many years.", Language = "English", Budget = 25000000 },
      new() { Id = Guid.Parse("d4fc99f6-eca9-4db9-98f7-dfc498c03727"), MovieId = Guid.Parse("7f91d565-521e-4ffd-a640-f7b52bf2c4cb"), Synopsis = "The aging patriarch of a crime family transfers control to his son.", Language = "English", Budget = 6000000 },
      new() { Id = Guid.Parse("7b1d676f-ee1f-46a6-9142-0d975e055006"), MovieId = Guid.Parse("2618aba7-9333-4e57-8ab2-334f6029ad97"), Synopsis = "Batman faces a criminal who brings chaos to Gotham.", Language = "English", Budget = 185000000 },
      new() { Id = Guid.Parse("49f27792-4139-48d8-ba9d-44776d6affed"), MovieId = Guid.Parse("a7b93843-4023-4165-a1fd-733b23c04c13"), Synopsis = "Several interconnected stories unfold in Los Angeles.", Language = "English", Budget = 8000000 },
      new() { Id = Guid.Parse("ff1d9c12-1a38-47dc-bf62-2caf95362f7b"), MovieId = Guid.Parse("2e51aa7e-9a7a-46a4-b859-da7cbef6f370"), Synopsis = "A man recounts the extraordinary events of his life.", Language = "English", Budget = 55000000 },
      new() { Id = Guid.Parse("e59a2896-c4e3-4280-8a10-6114b08e2ca8"), MovieId = Guid.Parse("7297e5a2-60a6-423c-9550-144cd3b378a1"), Synopsis = "A team enters dreams to carry out a difficult mission.", Language = "English", Budget = 160000000 },
      new() { Id = Guid.Parse("20fd04eb-e379-42c4-88fa-2479f3acc14f"), MovieId = Guid.Parse("8da7a5f1-fe0d-44c2-a8a9-c0ec554b3b83"), Synopsis = "A hacker discovers that reality is not what it seems.", Language = "English", Budget = 63000000 },
      new() { Id = Guid.Parse("a025e4f1-8eda-46fc-873a-f5b812f1be2e"), MovieId = Guid.Parse("3525e0fb-a297-4fd8-acb4-0ab1a7ef00e0"), Synopsis = "Explorers travel through a wormhole in search of a future for humanity.", Language = "English", Budget = 165000000 },
      new() { Id = Guid.Parse("cfb05465-95b5-4d1f-af56-51dbe99f849f"), MovieId = Guid.Parse("0500a043-71d6-4fc6-a819-08208b326db6"), Synopsis = "A poor family becomes entangled with a wealthy household.", Language = "Korean", Budget = 11400000 },
      new() { Id = Guid.Parse("0d35f875-ebbf-4f6b-b973-91e4c07b5303"), MovieId = Guid.Parse("db64cc44-02fb-4e55-8c77-a117c15dbea7"), Synopsis = "A girl enters a mysterious world of spirits.", Language = "Japanese", Budget = 19000000 }
    };

    var reviews = new List<Review>
    {
      new() { Id = new Guid("98a7abee-e119-4942-8a3c-9ff0a723ab46"), MovieId = Guid.Parse("261bb26e-3ddc-426a-8f2b-fbf8d1fd9718"), ReviewerName = "Anna", Comment = "En gripande film.", Rating = 5 },
      new() { Id = new Guid("98a8a84e-0b8e-4a0c-bc9e-0e2bdd9067fd"), MovieId = Guid.Parse("7f91d565-521e-4ffd-a640-f7b52bf2c4cb"), ReviewerName = "Erik", Comment = "En klassiker med starka rollprestationer.", Rating = 5 },
      new() { Id = new Guid("9a2ad4a9-cd93-4853-be2b-2ec673727187"), MovieId = Guid.Parse("2618aba7-9333-4e57-8ab2-334f6029ad97"), ReviewerName = "Sara", Comment = "Spännande från början till slut.", Rating = 5 },
      new() { Id = new Guid("00c1c428-d48f-4b51-b868-4561e07276b6"), MovieId = Guid.Parse("a7b93843-4023-4165-a1fd-733b23c04c13"), ReviewerName = "Johan", Comment = "Originell berättarstil.", Rating = 4 },
      new() { Id = new Guid("0904ac71-808f-48ab-bb5e-9c11c843d69c"), MovieId = Guid.Parse("2e51aa7e-9a7a-46a4-b859-da7cbef6f370"), ReviewerName = "Maja", Comment = "Rolig och känslosam.", Rating = 5 },
      new() { Id = new Guid("04cc720a-7215-4c0a-bdb7-ddb9f92afec8"), MovieId = Guid.Parse("7297e5a2-60a6-423c-9550-144cd3b378a1"), ReviewerName = "Oskar", Comment = "Kräver koncentration, men är sevärd.", Rating = 4 },
      new() { Id = new Guid("27ff8c39-827f-4fd1-88ae-ed4e844e227c"), MovieId = Guid.Parse("8da7a5f1-fe0d-44c2-a8a9-c0ec554b3b83"), ReviewerName = "Linnea", Comment = "En minnesvärd sci-fi-film.", Rating = 5 },
      new() { Id = new Guid("6acc8b74-e64a-4782-899b-622680e70837"), MovieId = Guid.Parse("3525e0fb-a297-4fd8-acb4-0ab1a7ef00e0"), ReviewerName = "David", Comment = "Visuellt imponerande.", Rating = 5 },
      new() { Id = new Guid("4850a398-51db-4ed5-abb9-335bde65d8cb"), MovieId = Guid.Parse("0500a043-71d6-4fc6-a819-08208b326db6"), ReviewerName = "Nora", Comment = "Överraskande och välgjord.", Rating = 5 },
      new() { Id = new Guid("01875b85-8e3a-41b7-82a9-9636a0e7682f"), MovieId = Guid.Parse("db64cc44-02fb-4e55-8c77-a117c15dbea7"), ReviewerName = "Emil", Comment = "Vacker och fantasifull.", Rating = 5 }
    };

    var movieActors = new List<MovieActor>
    {
      // The Shawshank Redemption
      new() { Id = Guid.NewGuid(), MovieId = Guid.Parse("261bb26e-3ddc-426a-8f2b-fbf8d1fd9718"), ActorId = Guid.Parse("f4008e34-6dd8-41bf-a454-27f4c063f12b"), Role = "Andy Dufresne" },
      new() { Id = Guid.NewGuid(), MovieId = Guid.Parse("261bb26e-3ddc-426a-8f2b-fbf8d1fd9718"), ActorId = Guid.Parse("39f9388d-1dd3-4f66-8883-9ca94902d839"), Role = "Ellis Boyd Redding" },

      // The Godfather
      new() { Id = Guid.NewGuid(), MovieId = Guid.Parse("7f91d565-521e-4ffd-a640-f7b52bf2c4cb"), ActorId = Guid.Parse("ca52092b-dd2b-4426-9151-e141c7d3d17a"), Role = "Vito Corleone" },
      new() { Id = Guid.NewGuid(), MovieId = Guid.Parse("7f91d565-521e-4ffd-a640-f7b52bf2c4cb"), ActorId = Guid.Parse("dba2a269-f2ff-400a-9586-cacf03266750"), Role = "Michael Corleone" },

      // The Dark Knight
      new() { Id = Guid.NewGuid(), MovieId = Guid.Parse("2618aba7-9333-4e57-8ab2-334f6029ad97"), ActorId = Guid.Parse("af65590f-fdf1-4010-afee-67c2d0cf93d6"), Role = "Bruce Wayne / Batman" },
      new() { Id = Guid.NewGuid(), MovieId = Guid.Parse("2618aba7-9333-4e57-8ab2-334f6029ad97"), ActorId = Guid.Parse("d16be9e7-639a-43fe-b811-cc0d8d8c9195"), Role = "The Joker" },

      // Pulp Fiction
      new() { Id = Guid.NewGuid(), MovieId = Guid.Parse("a7b93843-4023-4165-a1fd-733b23c04c13"), ActorId = Guid.Parse("1b91ea44-015b-4e01-ae37-6c75847d620b"), Role = "Vincent Vega" },

      // Forrest Gump
      new() { Id = Guid.NewGuid(), MovieId = Guid.Parse("2e51aa7e-9a7a-46a4-b859-da7cbef6f370"), ActorId = Guid.Parse("ade5edea-c0bf-42ef-9fba-b6b71c5b4551"), Role = "Forrest Gump" },

      // Inception
      new() { Id = Guid.NewGuid(), MovieId = Guid.Parse("7297e5a2-60a6-423c-9550-144cd3b378a1"), ActorId = Guid.Parse("c8f5eed4-385e-4aae-bf86-f9a92e2e3f95"), Role = "Dom Cobb" },

      // The Matrix
      new() { Id = Guid.NewGuid(), MovieId = Guid.Parse("8da7a5f1-fe0d-44c2-a8a9-c0ec554b3b83"), ActorId = Guid.Parse("af076ba2-12c3-46c3-aab2-5b55bbfa6880"), Role = "Neo" }
    };

    var movieGenres = new List<MovieGenre>
    {
      // The Shawshank Redemption – Drama
      new() { MovieId = Guid.Parse("261bb26e-3ddc-426a-8f2b-fbf8d1fd9718"), GenreId = Guid.Parse("8f68b2b1-dec2-4b33-86b7-969affad5ef2") },
  
      // The Godfather – Crime, Drama
      new() { MovieId = Guid.Parse("7f91d565-521e-4ffd-a640-f7b52bf2c4cb"), GenreId = Guid.Parse("b8b6ed01-3200-44f6-b663-eef936328e76") },
      new() { MovieId = Guid.Parse("7f91d565-521e-4ffd-a640-f7b52bf2c4cb"), GenreId = Guid.Parse("8f68b2b1-dec2-4b33-86b7-969affad5ef2") },
  
      // The Dark Knight – Action, Crime
      new() { MovieId = Guid.Parse("2618aba7-9333-4e57-8ab2-334f6029ad97"), GenreId = Guid.Parse("7e085268-ae56-436f-baaf-5f496e084cd1") },
      new() { MovieId = Guid.Parse("2618aba7-9333-4e57-8ab2-334f6029ad97"), GenreId = Guid.Parse("b8b6ed01-3200-44f6-b663-eef936328e76") },
  
      // Pulp Fiction – Crime
      new() { MovieId = Guid.Parse("a7b93843-4023-4165-a1fd-733b23c04c13"), GenreId = Guid.Parse("b8b6ed01-3200-44f6-b663-eef936328e76") },
  
      // Forrest Gump – Drama
      new() { MovieId = Guid.Parse("2e51aa7e-9a7a-46a4-b859-da7cbef6f370"), GenreId = Guid.Parse("8f68b2b1-dec2-4b33-86b7-969affad5ef2") },
  
      // Inception – Sci-Fi
      new() { MovieId = Guid.Parse("7297e5a2-60a6-423c-9550-144cd3b378a1"), GenreId = Guid.Parse("cd868400-c90a-423f-ac3d-659e5bfd4173") },
  
      // The Matrix – Sci-Fi
      new() { MovieId = Guid.Parse("8da7a5f1-fe0d-44c2-a8a9-c0ec554b3b83"), GenreId = Guid.Parse("cd868400-c90a-423f-ac3d-659e5bfd4173") },
  
      // Interstellar – Sci-Fi
      new() { MovieId = Guid.Parse("3525e0fb-a297-4fd8-acb4-0ab1a7ef00e0"), GenreId = Guid.Parse("cd868400-c90a-423f-ac3d-659e5bfd4173") },
  
      // Parasite – Drama, Crime
      new() { MovieId = Guid.Parse("0500a043-71d6-4fc6-a819-08208b326db6"), GenreId = Guid.Parse("8f68b2b1-dec2-4b33-86b7-969affad5ef2") },
      new() { MovieId = Guid.Parse("0500a043-71d6-4fc6-a819-08208b326db6"), GenreId = Guid.Parse("b8b6ed01-3200-44f6-b663-eef936328e76") },
  
      // Spirited Away – Animation
      new() { MovieId = Guid.Parse("db64cc44-02fb-4e55-8c77-a117c15dbea7"), GenreId = Guid.Parse("5ea1a68f-a643-44b8-9848-dc40eaf539ff") }
    };

    context.AddRange(movies);
    context.AddRange(actors);
    context.AddRange(genres);
    context.AddRange(details);
    context.AddRange(reviews);
    context.AddRange(movieActors);
    context.AddRange(movieGenres);

    context.SaveChanges();
  }
}