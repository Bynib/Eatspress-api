using Microsoft.AspNetCore.Mvc;
using Eatspress.Models;
using Eatspress.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eatspress.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartDetailsController : ControllerBase
    {
        private readonly CartDetailsService _service;

        public CartDetailsController(CartDetailsService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<CartDetails>> GetAll() => await _service.GetAllAsync();

        [HttpGet("{orderId}/{itemId}")]
        public async Task<ActionResult<CartDetails>> GetById(int orderId, int itemId)
        {
            var details = await _service.GetByIdAsync(orderId, itemId);
            if (details == null) return NotFound();
            return details;
        }

        [HttpPost]
        public async Task<ActionResult<CartDetails>> Create(CartDetails details)
        {
            var created = await _service.CreateAsync(details);
            return CreatedAtAction(nameof(GetById), new { orderId = created.Order_Id, itemId = created.Item_Id }, created);
        }

        [HttpPut("{orderId}/{itemId}")]
        public async Task<ActionResult<CartDetails>> Update(int orderId, int itemId, CartDetails details)
        {
            var updated = await _service.UpdateAsync(orderId, itemId, details);
            if (updated == null) return NotFound();
            return updated;
        }

        [HttpDelete("{orderId}/{itemId}")]
        public async Task<IActionResult> Delete(int orderId, int itemId)
        {
            var deleted = await _service.DeleteAsync(orderId, itemId);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
