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
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _svc;

        public OrderController(IOrderService svc)
        {
            _svc = svc;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderRequest req)
        {
            try
            {
                var id = await _svc.PlaceOrderAsync(req);
                return Ok(new { orderId = id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _svc.GetByIdAsync(id);
            if (order == null) return NotFound(new { message = "Order not found" });
            return Ok(order);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var orders = await _svc.GetByUserAsync(userId);
            return Ok(orders);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _svc.GetAllAsync();
            return Ok(orders);
        }

        [HttpPut("{orderId}/status/{statusId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int orderId, int statusId)
        {
            var ok = await _svc.UpdateStatusAsync(orderId, statusId);
            if (!ok) return NotFound(new { message = "Order not found" });
            return Ok(new { message = "Status updated" });
        }

        [HttpDelete("{orderId}")]
        public async Task<IActionResult> Cancel(int orderId)
        {
            var ok = await _svc.CancelAsync(orderId);
            if (!ok) return NotFound(new { message = "Order not found" });
            return Ok(new { message = "Order canceled" });
        }
    }
}
