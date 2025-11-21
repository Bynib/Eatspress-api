using Eatspress.Data;
using Eatspress.Interfaces;
using Eatspress.Models;
using Eatspress.ServiceModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Eatspress.Services
{
    public class MenuService : IMenuService
    {
        private readonly AppDBContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly string _imgPath;
        private readonly IValidationService _v;

        public MenuService(AppDBContext db, IWebHostEnvironment env, IValidationService v)
        {
            _v = v;
            _db = db;
            _env = env;
            _imgPath = Path.Combine(_env.ContentRootPath, "Resources");
            if (!Directory.Exists(_imgPath))
                Directory.CreateDirectory(_imgPath);
        }

        public async Task<FoodItemResponse> CreateAsync(FoodItemRequest req)
        {
            if(!await _v.IsValidImageAsync(req.Image!)) 
                throw new Exception("Invalid file. Must be an image and a maximum of 2MB");
            
            if(req.Price < 1)
                throw new Exception("Price must be greater than zero");
            if(req.Prep_Time < 1)
                throw new Exception("Prep Time must be greater than zero");
            var food = new FoodItem
            {
                Name = req.Name,
                Description = req.Description,
                Prep_Time = req.Prep_Time,
                Category_Id = req.Category_Id,
                Price = req.Price,
                Created_At = DateTime.UtcNow
            };

            _db.FoodItems.Add(food);
            await _db.SaveChangesAsync();

            if (req.Image != null && req.Image.Length > 0)
            {
                var filePath = Path.Combine(_imgPath, $"{food.Item_Id}.jpg");
                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    await req.Image.CopyToAsync(fs);
                }
            }

            return await MapToResponse(food);
        }

        public async Task<IEnumerable<FoodItemResponse>> GetAllAsync()
        {
            var foods = await _db.FoodItems
                .ToListAsync();

            var res = new List<FoodItemResponse>();
            foreach (var f in foods)
                res.Add(await MapToResponse(f));

            return res;
        }

        public async Task<FoodItemResponse?> GetByIdAsync(int id)
        {
            var food = await _db.FoodItems
                .FirstOrDefaultAsync(f => f.Item_Id == id);
            return food == null ? null : await MapToResponse(food);
        }

        public async Task<FoodItemResponse?> UpdateAsync(int id, FoodItemRequest req)
        {
            var food = await _db.FoodItems.FirstOrDefaultAsync(f => f.Item_Id == id);
            if (food == null) return null;
            Console.WriteLine(id);
            if(req.Price < 1)
                throw new Exception("Price must be greater than zero");
            if(req.Prep_Time < 1)
                throw new Exception("Prep Time must be greater than zero");
            if (!string.IsNullOrWhiteSpace(req.Name))
                food.Name = req.Name;

            if (!string.IsNullOrWhiteSpace(req.Description))
                food.Description = req.Description;

            if (req.Prep_Time > 0)
                food.Prep_Time = req.Prep_Time;

            if (req.Category_Id > 0)
                food.Category_Id = req.Category_Id;

            if (req.Price > 0)
                food.Price = req.Price;

            food.Updated_At = DateTime.UtcNow;

            Console.WriteLine("test");
            if (req.Image != null && req.Image.Length > 0)
            {
                var filePath = Path.Combine(_imgPath, $"{food.Item_Id}.jpg");
                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    await req.Image.CopyToAsync(fs);
                }
            }

            await _db.SaveChangesAsync();
            return await MapToResponse(food);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var food = await _db.FoodItems.FirstOrDefaultAsync(f => f.Item_Id == id && f.Deleted_At == null);
            if (food == null) return false;

            food.Deleted_At = DateTime.UtcNow;

            var carts = await _db.Carts
                .Where(c => c.Item_Id == id)
                .ToListAsync();

            if (carts.Any()) _db.Carts.RemoveRange(carts);
            await _db.SaveChangesAsync();

            return true;
        }

        private async Task<FoodItemResponse> MapToResponse(FoodItem f)
        {
            byte[]? imgBytes = null;
            var filePath = Path.Combine(_imgPath, $"{f.Item_Id}.jpg");
            if (File.Exists(filePath))
                imgBytes = await File.ReadAllBytesAsync(filePath);
            return new FoodItemResponse
            {
                Item_Id = f.Item_Id,
                Name = f.Name,
                Description = f.Description,
                Prep_Time = f.Prep_Time,
                Category_Id = f.Category_Id,
                Price = f.Price,
                Image = imgBytes,
                IsDeleted = f.Deleted_At != null
            };
        }
    }
}
