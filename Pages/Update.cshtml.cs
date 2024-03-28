using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FilmEntities;
using FilmContext;
using System.Diagnostics;
namespace Project.Pages
{
     public class UpdateFilm : PageModel
{
    private readonly FilmsDatabase _context;
    public UpdateFilm(FilmsDatabase context)
    {
        _context = context;
    }

    [BindProperty]
    public Film Film { get; set; }

    public IActionResult OnGet(int id) // Add 'id' parameter
    {
        Film = _context.films.FirstOrDefault(f => f.FilmId == id);

        if (Film == null)
        {
            return NotFound();
        }

        return Page();
    }

   public IActionResult OnPost()
{
    var stopwatch = new Stopwatch();
    stopwatch.Start();

      if (!int.TryParse(Request.Form["id"], out var filmId))
    {
        // Handle the case where the film ID is not a valid integer
        return BadRequest("Invalid film ID");
    }
    var film = _context.films.FirstOrDefault(f => f.FilmId == filmId);

    if (film == null)
    {
        return NotFound();
    }

    film.Title = Request.Form["tbxTitleUpdate"];
    film.Genre = Request.Form["tbxGenreUpdate"];
    film.Release_Date = Request.Form["tbxReleaseUpdate"];
    film.Age_Rating = Request.Form["tbxAgeUpdate"];
    film.Rating = Request.Form["tbxRatingUpdate"].ToString();

    // Assuming ActorID is a property of Film
    if (int.TryParse(Request.Form["tbxActorUpdate"], out var actorId))
    {
        film.ActorID = actorId;
    }
    else
    {
        // Handle the case where ActorID is not a valid integer
        return BadRequest("Invalid Actor ID");
    }

    _context.films.Update(film);
    _context.SaveChanges();

            stopwatch.Stop();
            Console.WriteLine($"Time taken to Update 1  record: {stopwatch.Elapsed.TotalSeconds} seconds");
  TempData["Message"] = $"Record successfully Update. \\n\\\n\\\n Time taken to Update 1 record: {stopwatch.Elapsed.TotalSeconds} seconds" ;
    return RedirectToPage("./Index");
}


}
}