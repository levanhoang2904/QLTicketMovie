using BookTicketMovie.Data;
using BookTicketMovie.Models;
using Microsoft.EntityFrameworkCore;

namespace BookTicketMovie.Services.Tickets
{
    public class TicketService : ICommonDataService<Ticket>
    {

        private readonly BookTicketMovieContext _context;

        public TicketService(BookTicketMovieContext context)
        {
            _context = context;
        }
        public async Task<Ticket?> CreateAsync(Ticket data)
        {
            await _context.Ticket.AddAsync(data);
            await _context.SaveChangesAsync();
            return data;

        }

        public Task<bool> DeleteByIdAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<Ticket?> EditAsync(Ticket data)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Ticket>> GetAllAsync()
        {
            return await _context.Ticket
               .Include(s => s.Showtime)
               .Include(s => s.Chair)
               .Include(s => s.Movie)
               .ToListAsync();
        }

        public Task<Ticket?> GetByIdAsync(int? Id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> isUsed(int? Id)
        {
            throw new NotImplementedException();
        }
    }
}
