using Eatspress.Data;
using Eatspress.Interfaces;
using Eatspress.Models;
using Eatspress.ServiceModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourApp.ServiceModels;

namespace Eatspress.Services
{
    public class CartService : ICartService
    {
        private readonly AppDBContext _db;

        public CartService(AppDBContext db)
        {
            _db = db;
        }

        public async Task<CartResponse> AddToCartAsync(CartRequest req)
        {
            var item = await _db.FoodItems.FindAsync(req.Item_Id);
            if (item == null) throw new Exception("Item not found");

            var cart = await _db.Carts
                .FirstOrDefaultAsync(c => c.User_id == req.User_Id && c.Item_Id == req.Item_Id);

            if (cart != null)
            {
                cart.Quantity += req.Quantity;
                cart.Updated_At = DateTime.UtcNow;
            }
            else
            {
                cart = new Cart
                {
                    User_id = req.User_Id,
                    Item_Id = req.Item_Id,
                    Quantity = req.Quantity,
                    Created_At = DateTime.UtcNow
                };
                _db.Carts.Add(cart);
            }

            await _db.SaveChangesAsync();

            return new CartResponse
            {
                Item_Id = cart.Item_Id,
                Item_Name = item.Name,
                Quantity = cart.Quantity
            };
        }

        public async Task<IEnumerable<CartResponse>> GetUserCartAsync(int userId)
        {
            return await _db.Carts
                .Include(c => c.FoodItem)
                .Where(c => c.User_id == userId)
                .Select(c => new CartResponse
                {
                    Item_Id = c.Item_Id,
                    Item_Name = c.FoodItem.Name,
                    Quantity = c.Quantity
                })
                .ToListAsync();
        }

        public async Task<CartResponse?> UpdateCartAsync(CartRequest req)
        {
            var cart = await _db.Carts
                .FirstOrDefaultAsync(c => c.User_id == req.User_Id && c.Item_Id == req.Item_Id);

            if (cart == null) return null;

            var item = await _db.FoodItems.FindAsync(req.Item_Id);
            if (item == null) throw new Exception("Item not found");

            cart.Quantity = req.Quantity;
            cart.Updated_At = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return new CartResponse
            {
                Item_Id = cart.Item_Id,
                Item_Name = item.Name,
                Quantity = cart.Quantity
            };
        }

        public async Task<bool> RemoveFromCartAsync(int userId, int itemId)
        {
            var cart = await _db.Carts
                .FirstOrDefaultAsync(c => c.User_id == userId && c.Item_Id == itemId);

            if (cart == null) return false;

            _db.Carts.Remove(cart);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAllFromCartAsync(int userId)
        {
            var carts = await _db.Carts
                .Where(c => c.User_id == userId)
                .ToListAsync();

            if (!carts.Any()) return false;

            _db.Carts.RemoveRange(carts);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
