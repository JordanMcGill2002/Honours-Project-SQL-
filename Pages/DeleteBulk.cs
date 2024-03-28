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
    public class DeleteBulk : PageModel
    {
        private readonly FilmsDatabase _context;

        [TempData]
        public string Message { get; set; } // Added this line

        public DeleteBulk(FilmsDatabase context)
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

                // Count the number of records before the deletion
                int initialCount = _context.films.Count();

                try
                {
                    for (int i = 0; i < recordCount; i++)
                    {
                        // Get all film IDs
                        var filmIds = _context.films.Select(f => f.FilmId).ToList();

                        // Choose a random film ID
                        var random = new Random();
                        int randomFilmId = filmIds[random.Next(filmIds.Count)];

                        // Find the film with the random ID
                        var filmToDelete = _context.films.Find(randomFilmId);

                        // Delete the film
                        if (filmToDelete != null)
                        {
                            _context.films.Remove(filmToDelete);
                            await _context.SaveChangesAsync();
                            Console.WriteLine($"Deleting film: {filmToDelete.Title}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

                stopwatch.Stop();

                // Count the number of records after the deletion
                int finalCount = _context.films.Count();

                Console.WriteLine($"Time taken to delete {recordCount} records: {stopwatch.Elapsed.TotalSeconds} seconds");

                Message = $"{recordCount} records were successfully deleted.\\n\\n\\nThere were {initialCount} records before the deletion and {finalCount} records after the deletion. \\n\\n Time taken to delete {recordCount} records: {stopwatch.Elapsed.TotalSeconds} seconds";

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
            string fileName = $"Delete_{recordCount}.txt";
            System.IO.File.WriteAllText(fileName, sb.ToString());

            return RedirectToAction("Index");
        }
    }
}