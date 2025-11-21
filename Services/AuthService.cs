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
        private readonly IValidationService _v;

        public AuthService(AppDBContext db, TokenProvider tokens, IValidationService v)
        {
            _db = db;
            _tokens = tokens;
            _v = v;
        }

        public async Task<AuthenticationResponse> RegisterAsync(RegistrationRequest req, HttpResponse res)
        {
            

            if (await _db.Users.AnyAsync(u => u.Email == req.Email))
                throw new Exception("Email already exists");

            if (req.Password != req.ConfirmPassword)
                throw new Exception("Passwords do not match");

            if(!await _v.IsValidEmailAsync(req.Email))
                    throw new Exception("Invalid email format");

            if(!await _v.IsValidPhoneAsync(req.Phone_No))
                    throw new Exception("Invalid phone number");
            if(req.Password.Length < 8)
                    throw new Exception("Password must be atleast 8 characters long");        
            var isFirst = !await _db.Users.AnyAsync();
            var user = new User
            {
                Firstname = req.Firstname,
                Lastname = req.Lastname,
                Email = req.Email,
                Phone_No = req.Phone_No,
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
                .FirstOrDefaultAsync(u => u.Email.ToLower() == req.Email.ToLower());
            Console.WriteLine(user);
            if (user == null || !BCrypt.Net.BCrypt.Verify(req.Password, user.Password))
                throw new Exception("Invalid email or password");

            var (access, refresh) = _tokens.GenerateTokens(user);

            // store refresh token in cookie
            _tokens.Store(res, refresh);

            return new AuthenticationResponse
            {
                token = access,
                user = MapUser(user)
            };
        }

        public async Task<AuthenticationResponse?> GoogleAsync(string email, string password, HttpResponse res)
        {
            var u = await _db.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            if (u == null)
                throw new Exception("Set your phone number and your password");

            var (access, refresh) = _tokens.GenerateTokens(u);

            // store refresh token in cookie
            _tokens.Store(res, refresh);

            return new AuthenticationResponse
            {
                token = access,
                user = MapUser(u)
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
            };
        }
    }
}
