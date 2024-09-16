using BookTicketMovie.Data;
using BookTicketMovie.Models;
using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;

namespace BookTicketMovie.Services.Genres
{
    public class GenreService : ICommonDataService<Genre>
    {
        private readonly BookTicketMovieContext _context;

        public GenreService(BookTicketMovieContext context)
        {
            _context = context;
        }
        public async Task<Genre?> CreateAsync(Genre data)
        {
            await _context.Genre.AddAsync(data);
            await _context.SaveChangesAsync();

            return data;
        }

        public async Task<bool> DeleteByIdAsync(int Id)
        {
            var genre = await this.GetByIdAsync(Id);
            if (genre != null)
            {
                 _context.Genre.Remove(genre);
                 await _context.SaveChangesAsync();
                 return true;
            }

            return false;
        }

        public async Task<Genre?> EditAsync(Genre genre)
        {
            if (_context.Genre.Any(e => e.Id == genre.Id))
            {
                var genreResult = _context.Genre.Update(genre);
                await _context.SaveChangesAsync();
                return genre;
            }
            return null;
        }

        public async Task<List<Genre>> GetAllAsync()
        {
            return await _context.Genre.ToListAsync();
        }

        public async Task<Genre?> GetByIdAsync(int? Id)
        {
            var genre = await _context.Genre.FirstOrDefaultAsync(x => x.Id == Id);

            if (genre == null) return null;
            return genre;
        }

        public Task<bool> isUsed(int? Id)
        {
            throw new NotImplementedException();
        }
    }
}
