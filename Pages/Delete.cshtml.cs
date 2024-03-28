using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using FilmEntities;
using FilmContext;

namespace Project.Pages
{
    public class DeleteFilm : PageModel
    {
        private readonly FilmsDatabase _context;

        public DeleteFilm(FilmsDatabase context)
        {
            _context = context;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Film delFilm = _context.films.Single(a => a.FilmId == Int32.Parse(Request.Form["hndFilmID"]));
            _context.films.Remove(delFilm);
            _context.SaveChanges();
            stopwatch.Stop();
            Console.WriteLine($"Time taken to insert Delete 1  record: {stopwatch.Elapsed.TotalSeconds} seconds");

            TempData["Message"] = $"Record successfully Deleted. {Environment.NewLine} Time taken to Insert 1 record: {stopwatch.Elapsed.TotalSeconds} seconds";

            return Redirect("~/Index");
        }
    }
}
