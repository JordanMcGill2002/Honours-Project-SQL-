using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using FilmEntities;
using FilmContext; //does connection/ extracts dataset

namespace Project.Pages
{
    public class ViewActors : PageModel
    {
        private readonly FilmsDatabase _context;

        public ViewActors(FilmsDatabase context)
        {
            _context = context;
        }

        public String Heading { get; set; }

        public List<Actor> Actors { get; set; }

        public List<Film> Films { get; set; }

        public List<FilmActor> FilmActor { get; set; }

        public string SearchQuery { get; set; } // For Title and Genre
        public string SearchRating { get; set; } // Separate search query for Rating

        public void OnGet(string searchQuery, string searchCriteria, string genre) // Added genre parameter here
{

var stopwatch = new Stopwatch();

stopwatch.Start();

    SearchQuery = searchQuery?.ToLower(); // Convert to lower case
    Heading = "Movies & Actors";

    var filmActorQuery = _context.actors.Join(_context.films,
           f => f.ActorID, a => a.ActorID,
           (f, a) => new FilmActor()
           {
               FilmId = a.FilmId,
               FName = f.Forename, //a = film table and f = actor table
               Title = a.Title,
               Genre = a.Genre,
               Release = a.Release_Date,
               SName = f.Surname,
               Age = a.Age_Rating,
               Rating = a.Rating,
           }
       );

    if (!string.IsNullOrEmpty(searchQuery))
    {
        switch (searchCriteria)
        {
            case "Title":
                filmActorQuery = filmActorQuery.Where(f => f.Title.ToLower().Contains(searchQuery));
                break;
            case "Rating":
                filmActorQuery = filmActorQuery.Where(f => f.Rating == searchQuery);
                break;
        }
    }

    if (!string.IsNullOrEmpty(genre))
    {
        filmActorQuery = filmActorQuery.Where(f => f.Genre == genre);
    }

    FilmActor = filmActorQuery.ToList();
    stopwatch.Stop();
    Console.WriteLine($"Time taken to search for results: {stopwatch.Elapsed.TotalSeconds} seconds. Number of records found: {FilmActor.Count}.");

}
    }}
