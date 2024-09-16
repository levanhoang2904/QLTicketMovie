using BookTicketMovie.Data;
using BookTicketMovie.Models;
using Microsoft.EntityFrameworkCore;

namespace BookTicketMovie.Services.Chairs
{
    public class ChairService : ICommonDataService<Chair>
    {
        private readonly BookTicketMovieContext _context;

        public ChairService(BookTicketMovieContext context) 
        {
            _context = context;   
        }
        public async Task<Chair?> CreateAsync(Chair data)
        {
            await _context.Chair.AddAsync(data);
            await _context.SaveChangesAsync();

            return data;
        }

        public async Task<bool> DeleteByIdAsync(int Id)
        {
            var chair = await this.GetByIdAsync(Id);
            if (chair != null)
            {
                _context.Chair.Remove(chair);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<Chair?> EditAsync(Chair data)
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

        public async Task<List<Chair>> GetAllAsync()
        {
            return await _context.Chair.ToListAsync();
        }

        public async Task<Chair?> GetByIdAsync(int? Id)
        {
            var chair = await _context.Chair.FirstOrDefaultAsync(x => x.Id == Id);

            if (chair == null) return null;
            return chair;
        }

        public Task<bool> isUsed(int? Id)
        {
            throw new NotImplementedException();
        }
    }
}
