using BookTicketMovie.Data;
using BookTicketMovie.Models;
using Microsoft.EntityFrameworkCore;

namespace BookTicketMovie.Services.ShowTimes
{
    public class ShowTimeService : ICommonDataService<Showtime>
    {
        private readonly BookTicketMovieContext _context;

        public ShowTimeService(BookTicketMovieContext context)
        {
            _context = context;
        }
        public async Task<Showtime?> CreateAsync(Showtime data)
        {
            await _context.Showtime.AddAsync(data);
          
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<bool> DeleteByIdAsync(int Id)
        {
            var showTime = await this.GetByIdAsync(Id);
            if (showTime != null)
            {
                _context.Showtime.Remove(showTime);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<Showtime?> EditAsync(Showtime data)
        {
            try
            {
                _context.Update(data);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Chair.Any(e => e.Id == data.Id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
            return data;
        }

        public async Task<List<Showtime>> GetAllAsync()
        {
            return await _context.Showtime
                .Include(s => s.Movie)
                .Include(s => s.Room)
                .ToListAsync();
        }

        public async Task<Showtime?> GetByIdAsync(int? Id)
        {
            return await _context.Showtime
                .Include(s => s.Movie)
                .Include(s => s.Room)
                .FirstOrDefaultAsync(s => s.Id == Id);
        }

        public async Task<bool> isUsed(int? Id)
        {
            var ticket = await _context.Ticket.FirstOrDefaultAsync(s => s.Id == Id);
            if (ticket == null) return true;
            return false;
        }
    }
}
