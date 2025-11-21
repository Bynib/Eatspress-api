using Eatspress.Data;
using Eatspress.Interfaces;
using Eatspress.Models;
using Eatspress.ServiceModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eatspress.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDBContext _db;

        public OrderService(AppDBContext db)
        {
            _db = db;
        }

        public async Task<int> PlaceOrderAsync(OrderRequest req)
        {
            foreach (var d in req.Details)
            {
                if(d.Quantity > 10) throw new Exception("Each item has a maximum of 10 per order");
                if(d.Quantity < 1) throw new Exception("Each item has a minimum of 1 per order");
            }
            var order = new Order
            {
                User_Id = req.User_Id,
                Address_Id = req.Address_Id,
                Instruction = req.Instruction,
                Estimated_Time = req.Estimated_Time,
                Status_Id = 1, // Pending by default
                Created_At = DateTime.UtcNow
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            foreach (var d in req.Details)
            {
                var od = new OrderDetails
                {
                    Order_Id = order.Order_Id,
                    Item_Id = d.Item_id,
                    Quantity = d.Quantity
                };
                _db.OrderDetails.Add(od);
            }

            await _db.SaveChangesAsync();
            return order.Order_Id;
        }

        public async Task<OrderResponse?> GetByIdAsync(int id)
        {
            var o = await _db.Orders
                .Include(x => x.User)
                .Include(x => x.Address)
                .Include(x => x.Status)
                .Include(x => x.OrderDetails).ThenInclude(d => d.FoodItem)
                .OrderByDescending(x => x.Created_At)
                .FirstOrDefaultAsync(x => x.Order_Id == id);

            return o == null ? null : MapOrder(o);
        }

        public async Task<IEnumerable<OrderResponse>> GetByUserAsync(int userId)
        {
            var orders = await _db.Orders
                .Include(x => x.Status)
                .Include(x => x.OrderDetails).ThenInclude(d => d.FoodItem)
                .Where(x => x.User_Id == userId)
                .OrderByDescending(x => x.Created_At)
                .ToListAsync();

            return orders.Select(MapOrder);
        }

        public async Task<IEnumerable<OrderResponse>> GetAllAsync()
        {
            var orders = await _db.Orders
                .Include(x => x.Status)
                .Include(x => x.OrderDetails).ThenInclude(d => d.FoodItem)
                .OrderByDescending(x => x.Created_At)
                .ToListAsync();

            return orders.Select(MapOrder);
        }

        public async Task<bool> UpdateStatusAsync(int orderId, int statusId)
        {
            var o = await _db.Orders.FindAsync(orderId);
            if (o == null) return false;

            o.Status_Id = statusId;
            o.Updated_At = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelAsync(int orderId)
        {
            var o = await _db.Orders.FindAsync(orderId);
            if (o == null) return false;

            o.Status_Id = 5; // e.g., Canceled
            o.Updated_At = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        private OrderResponse MapOrder(Order o)
        {
            return new OrderResponse
            {
                Order_Id = o.Order_Id,
                User_Id = o.User_Id,
                Address_Id = o.Address_Id,
                Status_Id = o.Status_Id,
                Instruction = o.Instruction,
                Estimated_Time = o.Estimated_Time,
                Created_At = o.Created_At,
                Details = o.OrderDetails.Select(d => new OrderDetailsResponse
                {
                    Item_Id = d.Item_Id,
                    Quantity = d.Quantity,
                }).ToList()
            };
        }
    }
}
    