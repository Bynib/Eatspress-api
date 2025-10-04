using BCrypt.Net;
using Eatspress.Authentication;
using Eatspress.Data;
using Eatspress.Interfaces;
using Eatspress.Models;
using Eatspress.ServiceModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Eatspress.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDBContext _db;
        private readonly TokenProvider _tokens;

        public AuthService(AppDBContext db, TokenProvider tokens)
        {
            _db = db;
            _tokens = tokens;
        }

        public async Task<AuthenticationResponse> RegisterAsync(RegistrationRequest req, HttpResponse res)
        {
            if (req.Password != req.ConfirmPassword)
                throw new Exception("Passwords do not match");

            if (await _db.Users.AnyAsync(u => u.Username == req.Username))
                throw new Exception("Username already exists");

            if (await _db.Users.AnyAsync(u => u.Email == req.Email))
                throw new Exception("Email already exists");

            var isFirst = !await _db.Users.AnyAsync();
            var user = new User
            {
                Firstname = req.Firstname,
                Lastname = req.Lastname,
                Email = req.Email,
                Phone_No = req.Phone_No,
                Username = req.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(req.Password),
                Role_Id =  isFirst ? 1: 2,
                Created_At = DateTime.UtcNow
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            var (access, refresh) = _tokens.GenerateTokens(user);

            // store refresh token in cookie
            _tokens.Store(res, refresh);

            return new AuthenticationResponse
            {
                token = access,
                user = MapUser(user)
            };
        }

        public async Task<AuthenticationResponse> LoginAsync(LoginRequest req, HttpResponse res)
        {
            var user = await _db.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username.ToLower() == req.Username.ToLower());
            Console.WriteLine(user);
            if (user == null || !BCrypt.Net.BCrypt.Verify(req.Password, user.Password))
                throw new Exception("Invalid username or password");

            var (access, refresh) = _tokens.GenerateTokens(user);

            // store refresh token in cookie
            _tokens.Store(res, refresh);

            return new AuthenticationResponse
            {
                token = access,
                user = MapUser(user)
            };
        }

        public string Refresh(string refreshToken)
        {
            var newAccess = _tokens.Refresh(refreshToken);
            if (newAccess == null) throw new Exception("Invalid refresh token");
            return newAccess;
        }

        public void Logout(HttpRequest req,HttpResponse res)
        {
            _tokens.Delete(req,res);
        }

        private UserResponse MapUser(User u)
        {
            return new UserResponse
            {
                User_Id = u.User_Id,
                Firstname = u.Firstname,
                Lastname = u.Lastname,
                Email = u.Email,
                Phone_No = u.Phone_No,
                Username = u.Username
            };
        }
    }
}
