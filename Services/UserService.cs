// Services/UserService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eatspress.Data;
using Eatspress.Interfaces;
using Eatspress.Models;
using Eatspress.ServiceModels;
using Microsoft.EntityFrameworkCore;

namespace Eatspress.Services
{
    public class UserService : IUserService
    {
        private readonly AppDBContext _context;

        public UserService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<UserResponse> UpdateUserAsync(UpdateUserRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.User_Id == request.User_Id);
            if (user == null) throw new Exception("User not found");

            if (!string.IsNullOrWhiteSpace(request.Firstname)) user.Firstname = request.Firstname;
            if (!string.IsNullOrWhiteSpace(request.Lastname)) user.Lastname = request.Lastname;
            if (!string.IsNullOrWhiteSpace(request.Email)) user.Email = request.Email;
            if (!string.IsNullOrWhiteSpace(request.Phone_No)) user.Phone_No = request.Phone_No;
            if (!string.IsNullOrWhiteSpace(request.Username)) user.Username = request.Username;

            if (request.Address_Id > 0) user.Address_Id = request.Address_Id;

            if (!string.IsNullOrWhiteSpace(request.OldPassword) &&
                !string.IsNullOrWhiteSpace(request.NewPassword) &&
                !string.IsNullOrWhiteSpace(request.ConfirmPassword))
            {
                if (!BCrypt.Net.BCrypt.Verify(request.OldPassword, user.Password))
                    throw new Exception("Old password is incorrect");

                if (request.NewPassword != request.ConfirmPassword)
                    throw new Exception("New password and confirm password do not match");

                user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            }

            user.Updated_At = DateTime.UtcNow;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return new UserResponse
            {
                User_Id = user.User_Id,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                Phone_No = user.Phone_No,
                Username = user.Username
            };
        }

        public async Task<IEnumerable<UserResponse>> GetAllAsync()
        {
            return await _context.Users
                .Select(u => new UserResponse
                {
                    User_Id = u.User_Id,
                    Firstname = u.Firstname,
                    Lastname = u.Lastname,
                    Email = u.Email,
                    Phone_No = u.Phone_No,
                    Username = u.Username
                })
                .ToListAsync();
        }

        public async Task<UserResponse> GetByIdAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.User_Id == id);
            if (user == null) throw new Exception("User not found");

            return new UserResponse
            {
                User_Id = user.User_Id,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                Phone_No = user.Phone_No,
                Username = user.Username
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.User_Id == id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
