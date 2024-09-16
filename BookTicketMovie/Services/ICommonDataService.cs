namespace BookTicketMovie.Services
{
    public interface ICommonDataService<T> where T : class
    {
        public Task<List<T>> GetAllAsync();

        public Task<T?> GetByIdAsync(int? Id);

        public Task<T?> CreateAsync(T data);

        public Task<T?> EditAsync(T data);

        public Task<bool> DeleteByIdAsync(int Id);

        public Task<bool> isUsed(int? Id);
    }
}
