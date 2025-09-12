using Microsoft.AspNetCore.Mvc;
using Eatspress.Models;
using Eatspress.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eatspress.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _service;

        public OrderController(OrderService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<Order>> GetAll() => await _service.GetAllAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetById(int id)
        {
            var order = await _service.GetByIdAsync(id);
            if (order == null) return NotFound();
            return order;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> Create(Order order)
        {
            var created = await _service.CreateAsync(order);
            return CreatedAtAction(nameof(GetById), new { id = created.Order_Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Order>> Update(int id, Order order)
        {
            var updated = await _service.UpdateAsync(id, order);
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
