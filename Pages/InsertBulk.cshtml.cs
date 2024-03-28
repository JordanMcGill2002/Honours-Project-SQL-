using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using FilmEntities;
using FilmContext;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments; //does connection/ extracts dataset
using System.IO;
using System.Text;

namespace Project.Pages
{
    public class InsertBulk : PageModel
    {
        private readonly FilmsDatabase _context;

        [TempData]
        public string Message { get; set; } // Added this line

        public InsertBulk(FilmsDatabase context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return RedirectToPage("Index");
        }

        public async Task<IActionResult> OnPost(int recordCount)
{
    List<double> timesTaken = new List<double>();
    for (int j = 0; j < 10; j++)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var films = new List<Film>();
        var random = new Random();
        var genres = new[] { "Horror", "Action", "Sci-Fi", "Comedy", "Romance", "Musical", "Thriller" };
        var ageRatings = new[] { "U", "PG", "12-A", "15", "18" };
        int range = (DateTime.Today - new DateTime(DateTime.Today.Year - 10, 1, 1)).Days;

        // Count the number of records before the insertion
        int initialCount = _context.films.Count();

        // Retrieve all film titles that start with "Film " from the database
        var filmTitles = _context.films
            .Where(f => f.Title.StartsWith("Film "))
            .Select(f => f.Title)
            .ToList();

        // Parse the numbers from the film titles in memory
        int maxFilmNumber = filmTitles
            .Select(title => int.Parse(title.Substring(5)))
            .DefaultIfEmpty(0)
            .Max();

        for (int i = maxFilmNumber + 1; i <= maxFilmNumber + recordCount; i++)
        {
            var film = new Film
            {
                Title = $"Film {i}",
                Genre = genres[random.Next(genres.Length)],
                Release_Date = new DateTime(DateTime.Today.Year - 10, 1, 1).AddDays(random.Next(range)).ToString("dd/MM/yyyy"),
                Age_Rating = ageRatings[random.Next(ageRatings.Length)],
                Rating = random.Next(0, 11).ToString(),
                ActorID = random.Next(1, 11)
            };

            films.Add(film);
            Console.WriteLine($"Inserting new film: {film.Title}");
        }

        try
        {
            _context.films.AddRange(films);

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        stopwatch.Stop();

        // Count the number of records after the insertion
        int finalCount = _context.films.Count();

        Console.WriteLine($"Time taken to insert {recordCount} records: {stopwatch.Elapsed.TotalSeconds} seconds");

        Message = $"{recordCount} records were successfully inserted.\\n\\n\\nThere were {initialCount} records before the insertion and {finalCount} records after the insertion. \\n\\n Time taken to insert {recordCount} records: {stopwatch.Elapsed.TotalSeconds} seconds";

        timesTaken.Add(stopwatch.Elapsed.TotalSeconds);
    }

    StringBuilder sb = new StringBuilder();
    sb.AppendLine("Times taken for each iteration:");
    for (int i = 0; i < timesTaken.Count; i++)
    {
        sb.AppendLine($"{timesTaken[i]} ");
    }
    Console.WriteLine(sb.ToString());

    // Write the times taken to a text file
    string fileName = $"Insert_{recordCount}.txt";
    System.IO.File.WriteAllText(fileName, sb.ToString());

    return RedirectToAction("Index");
}
    }}