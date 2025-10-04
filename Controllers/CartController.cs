using Eatspress.Interfaces;
using YourApp.ServiceModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Eatspress.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _svc;

        public CartController(ICartService svc)
        {
            _svc = svc;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CartRequest req)
        {
            try
            {
                var cart = await _svc.AddToCartAsync(req);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserCart(int userId)
        {
            try
            {
                var cart = await _svc.GetUserCartAsync(userId);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CartRequest req)
        {
            try
            {
                var cart = await _svc.UpdateCartAsync(req);
                if (cart == null) return NotFound(new { message = "Cart item not found" });
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{userId}/{itemId}")]
        public async Task<IActionResult> Remove(int userId, int itemId)
        {
            try
            {
                var removed = await _svc.RemoveFromCartAsync(userId, itemId);
                if (!removed) return NotFound(new { message = "Cart item not found" });
                return Ok(new { message = "Removed successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
