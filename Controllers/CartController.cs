using Microsoft.AspNetCore.Mvc;
using Eatspress.Models;
using Eatspress.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eatspress.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly CartService _service;

        public CartController(CartService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<Cart>> GetAll() => await _service.GetAllAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetById(int id)
        {
            var cart = await _service.GetByIdAsync(id);
            if (cart == null) return NotFound();
            return cart;
        }

        [HttpPost]
        public async Task<ActionResult<Cart>> Create(Cart cart)
        {
            var created = await _service.CreateAsync(cart);
            return CreatedAtAction(nameof(GetById), new { id = created.Item_Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Cart>> Update(int id, Cart cart)
        {
            var updated = await _service.UpdateAsync(id, cart);
            if (updated == null) return NotFound();
            return updated;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
