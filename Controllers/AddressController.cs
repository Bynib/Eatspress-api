using Microsoft.AspNetCore.Mvc;
using Eatspress.Models;
using Eatspress.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eatspress.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly AddressService _service;

        public AddressController(AddressService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<Address>> GetAll() => await _service.GetAllAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Address>> GetById(int id)
        {
            var address = await _service.GetByIdAsync(id);
            if (address == null) return NotFound();
            return address;
        }

        [HttpPost]
        public async Task<ActionResult<Address>> Create(Address address)
        {
            var created = await _service.CreateAsync(address);
            return CreatedAtAction(nameof(GetById), new { id = created.Address_Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Address>> Update(int id, Address address)
        {
            var updated = await _service.UpdateAsync(id, address);
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
