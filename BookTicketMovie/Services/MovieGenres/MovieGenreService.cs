using System.Data;
using BookTicketMovie.Data;
using BookTicketMovie.Models;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookTicketMovie.Services.MovieGenres
{
    public class MovieGenreService : ICommonDataService<MovieEditView>
    {
        private readonly BookTicketMovieContext _context;

        public MovieGenreService(BookTicketMovieContext context)
        {
            _context = context; 
        }
        public async Task<MovieEditView?> CreateAsync(MovieEditView data)
        {
            if (data.idGenres.Any()) {
                foreach (var item in data.idGenres)
                {
                    var movieGenre = new MovieGenre
                    {
                        MovieId = data.Movie.Id,
                        GenreId = item
                    };
                    await _context.MovieGenre.AddAsync(movieGenre);
                }
                await _context.SaveChangesAsync();
                return data;
            }
            return null;
        }

        public async Task<bool> DeleteByIdAsync(int Id)
        {
            var movie = await _context.Movie.Include(m => m.MovieGenres).FirstOrDefaultAsync(m => m.Id == Id);
            if (movie != null)
            {
                _context.MovieGenre.RemoveRange(movie.MovieGenres!);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<MovieEditView?> EditAsync(MovieEditView data)
        {
            var movie = await _context.Movie.Include(m => m.MovieGenres).FirstOrDefaultAsync(m => m.Id == data.Movie.Id);
           if (movie != null)
            {
                _context.RemoveRange(movie.MovieGenres!);
               if (data.idGenres.Any())
                {
                    foreach(var item in data.idGenres)
                    {
                        var movieGenre = new MovieGenre
                        {
                            MovieId = movie.Id,
                            GenreId = item
                        };

                        _context.MovieGenre.Add(movieGenre);
                    }
                }
               await _context.SaveChangesAsync();
            }
            return data;
        }

        public Task<List<MovieEditView>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<MovieEditView?> GetByIdAsync(int? Id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> isUsed(int? Id)
        {
            throw new NotImplementedException();
        }
    }
}
