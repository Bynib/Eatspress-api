using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eatspress.Models;

namespace Eatspress.Services
{
    public class CartDetailsService
    {
        private readonly EatspressContext _context;

        public CartDetailsService(EatspressContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CartDetails>> GetAllAsync()
        {
            return await _context.CartDetails
                .Include(cd => cd.Order)
                .Include(cd => cd.Item)
                .ToListAsync();
        }

        public async Task<CartDetails> GetByIdAsync(int orderId, int itemId)
        {
            return await _context.CartDetails
                .Include(cd => cd.Order)
                .Include(cd => cd.Item)
                .FirstOrDefaultAsync(cd => cd.Order_Id == orderId && cd.Item_Id == itemId);
        }

        public async Task<CartDetails> CreateAsync(CartDetails details)
        {
            _context.CartDetails.Add(details);
            await _context.SaveChangesAsync();
            return details;
        }

        public async Task<CartDetails> UpdateAsync(int orderId, int itemId, CartDetails details)
        {
            var existing = await _context.CartDetails.FindAsync(orderId, itemId);
            if (existing == null) return null;

            existing.Instruction = details.Instruction;
            existing.Quantity = details.Quantity;
            existing.Updated_At = System.DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int orderId, int itemId)
        {
            var detail = await _context.CartDetails.FindAsync(orderId, itemId);
            if (detail == null) return false;

            _context.CartDetails.Remove(detail);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
