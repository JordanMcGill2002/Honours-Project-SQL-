using System;
using Microsoft.EntityFrameworkCore;
using FilmEntities;

namespace FilmContext
{

    public class FilmsDatabase : DbContext
    {
        public FilmsDatabase(DbContextOptions<FilmsDatabase> options)
        : base(options)
        {}
      public DbSet<Film> films {get; set;}// Define a dataset for each table AKA this line.. You also need a Entities file for each table.

      public DbSet<Actor> actors {get; set;}

   
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Films.db");
        }
    }

}






    
   
