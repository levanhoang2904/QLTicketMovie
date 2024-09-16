using BookTicketMovie.Models;

namespace BookTicketMovie.Services
{
    public interface ICommonUserService<T> where T : class
    {
        public Task<bool> MailIsExists(string email);

        public Task<T?>? Login(T user);

        
    }
}
