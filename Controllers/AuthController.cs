using Eatspress.Interfaces;
using Eatspress.ServiceModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Eatspress.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest req)
        {
            try
            {
                var res = await _auth.RegisterAsync(req, Response);
                return Ok(new { mesage = "Registered successfully", res.user, res.token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            try
            {
                var res = await _auth.LoginAsync(req, Response);
                return Ok(new { mesage = "Logged in successfully", res.user, res.token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("google")]
        public async Task<IActionResult> Google([FromBody] LoginRequest request)
        {
            try
            {
                var res = await _auth.GoogleAsync(request.Email, request.Password, Response);
                return Ok(new { token = res.token, user = res.user, message = "Logged In Successfully" });
            }catch(Exception ex) { return Ok(new { message = ex.Message, isNotSignedUp= true }); }
        }

        [HttpPost("refresh")]
        [Authorize]
        public IActionResult Refresh()
        {
            try
            {
                var refresh = Request.Cookies["refreshToken"];
                if (string.IsNullOrEmpty(refresh))
                    return Unauthorized(new { message = "Missing refresh token" });

                var newAccess = _auth.Refresh(refresh);
                return Ok(new { token = newAccess });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            try
            {
                _auth.Logout(Request,Response);
                return Ok(new { message = "Logged out successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
