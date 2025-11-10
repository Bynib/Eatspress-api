using Eatspress.Interfaces;
using Eatspress.ServiceModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Security.Claims;
namespace Eatspress.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _svc;

        public AddressController(IAddressService svc)
        {
            _svc = svc;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddressRequest req)
        {
            try
            {
                Console.WriteLine(1);
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                Console.WriteLine(2);
                var addr = await _svc.CreateAsync(userId, req);
                Console.WriteLine(3);
                return Ok(addr);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var addr = await _svc.GetByIdAsync(id);
                if (addr == null) return NotFound();
                return Ok(addr);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            try
            {
                var list = await _svc.GetByUserAsync(userId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateAddressRequest req)
        {
            try
            {
                var addr = await _svc.UpdateAsync(req);
                return Ok(addr);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var ok = await _svc.DeleteAsync(id);
                if (!ok) return NotFound();
                return Ok(new { message = "Address deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
