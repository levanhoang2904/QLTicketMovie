using BookTicketMovie.Data;
using BookTicketMovie.Models;
using Microsoft.EntityFrameworkCore;

namespace BookTicketMovie.Services.Rooms
{
    public class RoomService : ICommonDataService<Room>
    {
        private readonly BookTicketMovieContext _context;

        public RoomService(BookTicketMovieContext context)
        {
             _context = context; 
        }
        public async Task<Room?> CreateAsync(Room data)
        {
            _context.Add(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<bool> DeleteByIdAsync(int Id)
        {
            var room = await _context.Room.FindAsync(Id);
            if (room != null)
            {
                _context.Room.Remove(room);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
           
        }

        public async Task<Room?> EditAsync(Room data)
        {
            try
            {
                _context.Update(data);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_context.Room.Any(e => e.Id == data.Id))
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

        public async Task<List<Room>> GetAllAsync()
        {
            return await _context.Room.ToListAsync();
        }

        public async Task<Room?> GetByIdAsync(int? Id)
        {
            var room = await _context.Room.FirstOrDefaultAsync(x => x.Id == Id);

            if (room == null) return null;
            return room;
        }

        public Task<bool> isUsed(int? Id)
        {
            throw new NotImplementedException();
        }
    }
}
