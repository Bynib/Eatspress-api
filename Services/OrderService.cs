using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eatspress.Models;

namespace Eatspress.Services
{
    public class OrderService
    {
        private readonly EatspressContext _context;

        public OrderService(EatspressContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.Status)
                .Include(o => o.CartDetails)
                    .ThenInclude(cd => cd.Item)
                .ToListAsync();
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Status)
                .Include(o => o.CartDetails)
                    .ThenInclude(cd => cd.Item)
                .FirstOrDefaultAsync(o => o.Order_Id == id);
        }

        public async Task<Order> CreateAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> UpdateAsync(int id, Order order)
        {
            var existing = await _context.Orders.FindAsync(id);
            if (existing == null) return null;

            existing.Status_Id = order.Status_Id;
            existing.Updated_At = System.DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
