using BookTicketMovie.Data;
using BookTicketMovie.Models;
using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;

namespace BookTicketMovie.Services.Movies
{
    public class MovieService : ICommonDataService<Movie>
    {
        private readonly BookTicketMovieContext _context;

        public MovieService(BookTicketMovieContext context)
        {
            _context = context;
        }
        public async Task<Movie?> CreateAsync(Movie movie)
        {
            await _context.Movie.AddAsync(movie);
            await _context.SaveChangesAsync();
            return movie;
        }

        public async Task<bool> DeleteByIdAsync(int Id)
        {
            var movie = await _context.Movie.FindAsync(Id);
            if (movie != null)
            {
                _context.Movie.Remove(movie);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Movie?> EditAsync(Movie data)
        {
            if (_context.Movie.Any(e => e.Id == data.Id))
            {
                var movieResult = _context.Movie.Update(data);
                await _context.SaveChangesAsync();
                return data;
            }
            return null;
        }

        public async Task<List<Movie>> GetAllAsync()
        {
            return await _context.Movie
                .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
                .Include(s => s.Showtimes)
                .ToListAsync();
        }

        public async Task<Movie?> GetByIdAsync(int? Id)
        {
            return await _context.Movie
                .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
                .Include(m => m.Showtimes)
                .FirstOrDefaultAsync(m => m.Id == Id);
        }

        public async Task<bool> isUsed(int? Id)
        {
           var showTimeMovie = await _context.Showtime.FirstOrDefaultAsync(m => m.MovieId == Id);
            if (showTimeMovie == null) return true;
            return false;
        }
    }
}
