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
    public class InsertFilm : PageModel
    {
        private readonly FilmsDatabase _context;

        public InsertFilm(FilmsDatabase context)
        {
            _context = context;
        }

        public IActionResult OnPost()
        {
            var stopwatch = new Stopwatch();
         stopwatch.Start();

            var film = new Film
            {
                Title = Request.Form["tbxTitle"],
                Genre = Request.Form["tbxGenre"],
                Release_Date = Request.Form["tbxRelease"],
                Age_Rating = Request.Form["tbxAge"],
                Rating = Request.Form["tbxRating"],
                ActorID = int.Parse(Request.Form["tbxActor"])
            };

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.films.Add(film);
            _context.SaveChanges();

            stopwatch.Stop();
            Console.WriteLine($"Time taken to insert 1  record: {stopwatch.Elapsed.TotalSeconds} seconds");

            TempData["Message"] = $"Record successfully Inserted.\\n\\n\\n Time taken to Insert 1 record: {stopwatch.Elapsed.TotalSeconds} seconds" ;

            return Redirect("~/Index");
        }
    }
}
