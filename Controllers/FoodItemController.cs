using Microsoft.AspNetCore.Mvc;
using Eatspress.Models;
using Eatspress.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eatspress.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodItemController : ControllerBase
    {
        private readonly FoodItemService _service;

        public FoodItemController(FoodItemService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<FoodItem>> GetAll() => await _service.GetAllAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<FoodItem>> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return item;
        }

        [HttpPost]
        public async Task<ActionResult<FoodItem>> Create(FoodItem item)
        {
            var created = await _service.CreateAsync(item);
            return CreatedAtAction(nameof(GetById), new { id = created.Item_Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<FoodItem>> Update(int id, FoodItem item)
        {
            var updated = await _service.UpdateAsync(id, item);
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
