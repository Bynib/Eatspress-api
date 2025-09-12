using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eatspress.Models;
using BCrypt.Net;

namespace Eatspress.Services
{
    public class UserService
    {
        private readonly EatspressContext _context;

        public UserService(EatspressContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.Address)
                .Include(u => u.Role)
                .ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Address)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.User_Id == id);
        }

        public async Task<User> CreateAsync(User user)
        {
            if (!string.IsNullOrEmpty(user.Password))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            }
            user.Created_At = System.DateTime.UtcNow;
            user.Updated_At = System.DateTime.UtcNow;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(int id, User user)
        {
            var existing = await _context.Users.FindAsync(id);
            if (existing == null) return null;

            existing.Firstname = user.Firstname;
            existing.Lastname = user.Lastname;
            existing.Email = user.Email;
            existing.Phone_No = user.Phone_No;
            existing.Address_Id = user.Address_Id;
            existing.Username = user.Username;

            if (!string.IsNullOrEmpty(user.Password))
            {
                existing.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            }

            existing.Role_Id = user.Role_Id;
            existing.Updated_At = System.DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        // Validate login with hashed password
        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return null;

            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.Password);
            return isValid ? user : null;
        }
    }
}
