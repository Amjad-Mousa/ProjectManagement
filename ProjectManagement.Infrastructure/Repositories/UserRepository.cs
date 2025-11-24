using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectManagement.Domain.Models;
using ProjectManagement.Infrastructure.Data;
using ProjectManagement.Domain.IRepositories;

namespace ProjectManagement.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(AppDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            _logger.LogInformation("Retrieved user with ID: {UserId}", id);
            return user;
        }

        public async Task<User?> GetByUserNameAsync(string userName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            _logger.LogInformation("Retrieved user with username: {UserName}", userName);
            return user;
        }

        public async Task<List<User>> GetAllAsync()
        {
            var users = await _context.Users.AsNoTracking().ToListAsync();
            _logger.LogInformation("Retrieved all users from the database.");
            return users;
        }

        public async Task<User> AddAsync(User user)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Added new user with ID: {UserId}", user.Id);
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null)
                throw new KeyNotFoundException("User not found");

            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;

            if (!string.IsNullOrEmpty(user.PasswordHash))
            {
                existingUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            }

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated user with ID: {UserId}", user.Id);
            return existingUser;
        }

        public async Task<bool> DeleteAsync(User user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (existingUser == null)
                return false;

            _context.Users.Remove(existingUser);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted user with ID: {UserId}", user.Id);
            return true;
        }
    }
}