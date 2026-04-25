using Backend.Models;

namespace Backend.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);
    }

    public class UserRepository : IUserRepository
    {
        // TODO: Inject DbContext when database is configured
        // private readonly IDbContext _context;

        public UserRepository()
        {
            // TODO: Initialize context
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            // TODO: Implement get all users from database
            return await Task.FromResult(new List<User>());
        }

        public async Task<User> GetByIdAsync(int id)
        {
            // TODO: Implement get user by id from database
            return await Task.FromResult(new User());
        }

        public async Task<User> CreateAsync(User user)
        {
            // TODO: Implement create user in database
            return await Task.FromResult(user);
        }

        public async Task<User> UpdateAsync(User user)
        {
            // TODO: Implement update user in database
            return await Task.FromResult(user);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            // TODO: Implement delete user from database
            return await Task.FromResult(true);
        }
    }
}
