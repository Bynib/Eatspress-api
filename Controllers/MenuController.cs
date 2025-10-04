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
    [Authorize]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _svc;

        public MenuController(IMenuService svc)
        {
            _svc = svc;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] FoodItemRequest req)
        {
            try
            {
                var res = await _svc.CreateAsync(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _svc.GetAllAsync();
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var res = await _svc.GetByIdAsync(id);
            if (res == null) return NotFound(new { message = "Food item not found" });
            return Ok(res);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromForm] FoodItemRequest req)
        {
            try
            {
                var res = await _svc.UpdateAsync(id, req);
                if (res == null) return NotFound(new { message = "Food item not found" });
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _svc.DeleteAsync(id);
            if (!deleted) return NotFound(new { message = "Food item not found" });
            return Ok(new { message = "Deleted successfully" });
        }
    }
}
