using BookTicketMovie.Data;
using BookTicketMovie.Models;
using Microsoft.EntityFrameworkCore;

namespace BookTicketMovie.Services.ShowTimes
{
    public class ShowTimeViewService : ICommonDataService<ShowTimeView>
    {
        private readonly BookTicketMovieContext _context;
        public ShowTimeViewService(BookTicketMovieContext context) 
        {
            _context = context;
        }
        
        public async Task<ShowTimeView?> CreateAsync(ShowTimeView data)
        {
            if (data.ListDateTimes != null) {
                foreach (var item in data.ListDateTimes)
                {
                    var ShowTime = new Showtime
                    {
                        MovieId = data.MovieId,
                        RoomId = data.RoomId,
                        DateTime = item
                    };
                    var result = _context.Showtime.AddAsync(ShowTime);
                    
                    await _context.SaveChangesAsync();
                }
            return data;
            }
            return data;
        }

        public Task<bool> DeleteByIdAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<ShowTimeView?> EditAsync(ShowTimeView data)
        {
            throw new NotImplementedException();
        }

        public Task<List<ShowTimeView>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ShowTimeView?> GetByIdAsync(int? Id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> isUsed(int? Id)
        {
            throw new NotImplementedException();
        }
    }
}
