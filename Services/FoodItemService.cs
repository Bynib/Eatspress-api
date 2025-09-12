using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eatspress.Models;

namespace Eatspress.Services
{
    public class FoodItemService
    {
        private readonly EatspressContext _context;

        public FoodItemService(EatspressContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FoodItem>> GetAllAsync()
        {
            return await _context.FoodItems
                .Include(fi => fi.Category)
                .ToListAsync();
        }

        public async Task<FoodItem> GetByIdAsync(int id)
        {
            return await _context.FoodItems
                .Include(fi => fi.Category)
                .FirstOrDefaultAsync(fi => fi.Item_Id == id);
        }

        public async Task<FoodItem> CreateAsync(FoodItem item)
        {
            _context.FoodItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<FoodItem> UpdateAsync(int id, FoodItem item)
        {
            var existing = await _context.FoodItems.FindAsync(id);
            if (existing == null) return null;

            existing.Name = item.Name;
            existing.Description = item.Description;
            existing.Prep_Time = item.Prep_Time;
            existing.Category_Id = item.Category_Id;
            existing.Price = item.Price;
            existing.Updated_At = System.DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.FoodItems.FindAsync(id);
            if (item == null) return false;

            _context.FoodItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
