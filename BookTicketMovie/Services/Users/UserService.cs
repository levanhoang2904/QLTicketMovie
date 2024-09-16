using BookTicketMovie.Data;
using BookTicketMovie.Models;
using Microsoft.EntityFrameworkCore;

namespace BookTicketMovie.Services.Users
{
    public class UserService : ICommonUserService<User>, ICommonDataService<User>
    {
        private readonly BookTicketMovieContext _context;
        
        public UserService(BookTicketMovieContext context)
        {
            _context = context;
        }

        public async Task<User?> CreateAsync(User data)
        {
            await _context.User.AddAsync(data);
            await _context.SaveChangesAsync(); 
            return data;
        }

        public Task<bool> DeleteByIdAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> EditAsync(User data)
        {
            try
            {
                _context.Update(data);
                await _context.SaveChangesAsync();
                return data;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.User.Any(e => e.Id == data.Id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public Task<List<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<User?> GetByIdAsync(int? Id)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.Id == Id);
        }

        public Task<bool> isUsed(int? Id)
        {
            throw new NotImplementedException();
        }

        public async Task<User?>? Login(User user)
        {
            var userDb = await _context.User.FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);
            return userDb;
        }

     

        public async Task<bool> MailIsExists(string email)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == email);
            return user != null;
        }
    }
}
