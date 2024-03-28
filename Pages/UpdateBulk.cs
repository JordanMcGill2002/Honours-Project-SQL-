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
    public class UpdateBulk : PageModel
    {
        private readonly FilmsDatabase _context;

        [TempData]
        public string Message { get; set; } // Added this line

        public UpdateBulk(FilmsDatabase context)
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

                var random = new Random();
                var genres = new[] { "Horror", "Action", "Sci-Fi", "Comedy", "Romance", "Musical", "Thriller" };
                var ageRatings = new[] { "U", "PG", "12-A", "15", "18" };

                // Count the number of records before the update
                int initialCount = _context.films.Count();

                List<string> updateMessages = new List<string>();

                try
                {
                    for (int i = 0; i < recordCount; i++)
                    {
                        // Get all film IDs
                        var filmIds = _context.films.Select(f => f.FilmId).ToList();

                        // Choose a random film ID
                        int randomFilmId = filmIds[random.Next(filmIds.Count)];

                        // Find the film with the random ID
                        var filmToUpdate = _context.films.Find(randomFilmId);

                        // Update the film
                        if (filmToUpdate != null)
                        {
                            string beforeUpdate = $"Before update: {filmToUpdate.Title}, {filmToUpdate.Genre}, {filmToUpdate.Release_Date}, {filmToUpdate.Age_Rating}, {filmToUpdate.Rating}, {filmToUpdate.ActorID}";

                            filmToUpdate.Genre = genres[random.Next(genres.Length)]; // Selects a random genre

                            // Generate a random date in the past 10 years
                            int range = (DateTime.Today - new DateTime(DateTime.Today.Year - 10, 1, 1)).Days;
                            filmToUpdate.Title = "Updated " + filmToUpdate.Title;
                            filmToUpdate.Release_Date = new DateTime(DateTime.Today.Year - 10, 1, 1).AddDays(random.Next(range)).ToString("dd/MM/yyyy");
                            filmToUpdate.Age_Rating = ageRatings[random.Next(ageRatings.Length)]; // Selects a random age rating
                            filmToUpdate.Rating = random.Next(0, 11).ToString(); // Generates a random number between 0 and 10
                            filmToUpdate.ActorID = random.Next(1, 11); // Generates a random number between 1 and 10

                            await _context.SaveChangesAsync();

                            string afterUpdate = $"After update: {filmToUpdate.Title}, {filmToUpdate.Genre}, {filmToUpdate.Release_Date}, {filmToUpdate.Age_Rating}, {filmToUpdate.Rating}, {filmToUpdate.ActorID}";

                            Console.WriteLine(beforeUpdate);
                            Console.WriteLine(afterUpdate);

                            updateMessages.Add($"{beforeUpdate}\\n{afterUpdate}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

                stopwatch.Stop();

                // Count the number of records after the update
                int finalCount = _context.films.Count();

                Console.WriteLine($"Time taken to update {recordCount} records: {stopwatch.Elapsed.TotalSeconds} seconds");

                Message = $"{recordCount} records were successfully updated.\\n\\n\\nThere were {initialCount} records before the update and {finalCount} records after the update. \\n\\n Time taken to update {recordCount} records: {stopwatch.Elapsed.TotalSeconds} seconds\\n\\nUpdates:\\n{string.Join("\\n\\n", updateMessages)}";

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
            string fileName = $"Update_{recordCount}.txt";
            System.IO.File.WriteAllText(fileName, sb.ToString());

            return RedirectToAction("Index");
        }
    }
}