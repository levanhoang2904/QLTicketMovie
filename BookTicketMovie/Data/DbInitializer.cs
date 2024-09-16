using BookTicketMovie.Data;
using BookTicketMovie.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;

namespace ContosoUniversity.Data
{
    public static class DbInitializer
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            using (var context = new BookTicketMovieContext(serviceProvider.GetRequiredService<DbContextOptions<BookTicketMovieContext>>()))
            {
                if (!context.Movie.Any())
                {
                    var movies = new Movie[]
                    {
                        new Movie
                        {
                            Title = "When Harry Met Sally",
                            ReleaseDate = DateTime.Parse("1989-2-12"),
                            Time = 90,
                           
                        },
                        new Movie
                        {
                            Title = "Ghostbusters ",
                            ReleaseDate = DateTime.Parse("1984-3-13"),
                            Time = 90,

                        }
                    };


                    // DB has been seeded
                }

                if (!context.Genre.Any())
                {
                    var genres = new Genre[]
                     {
                        new Genre{name="Tình cảm"},
                        new Genre{name="Hành động"},
                         new Genre{name="Khoa học viễn tưởng"},

                     };
                    foreach (Genre g in genres)
                    {
                        context.Genre.Add(g);
                    }
                    context.SaveChanges();
                }

            }


        }
    }
}