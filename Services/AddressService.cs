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
    public class AddressService : IAddressService
    {
        private readonly AppDBContext _db;

        public AddressService(AppDBContext db)
        {
            _db = db;
        }

        public async Task<AddressResponse> CreateAsync(int userId, AddressRequest req)
        {
            var addr = new Address
            {
                User_Id = userId,
                Unit_No = req.Unit_No,
                Street = req.Street,
                Barangay = req.Barangay,
                City = req.City,
                Zip_Code = req.Zip_Code,
                Created_At = DateTime.UtcNow
            };

            _db.Addresses.Add(addr);
            await _db.SaveChangesAsync();

            return MapToResponse(addr);
        }

        public async Task<AddressResponse?> GetByIdAsync(int id)
        {
            var addr = await _db.Addresses
                .FirstOrDefaultAsync(a => a.Address_Id == id && a.Deleted_At == null);

            return addr == null ? null : MapToResponse(addr);
        }

        public async Task<IEnumerable<AddressResponse>> GetByUserAsync(int userId)
        {
            var list = await _db.Addresses
                .Where(a => a.User_Id == userId && a.Deleted_At == null)
                .ToListAsync();

            return list.Select(MapToResponse);
        }

        public async Task<AddressResponse> UpdateAsync(UpdateAddressRequest req)
        {
            var addr = await _db.Addresses.FindAsync(req.Address_Id);
            if (addr == null || addr.Deleted_At != null)
                throw new Exception("Address not found");

            addr.Unit_No = req.Unit_No;
            addr.Street = req.Street;
            addr.Barangay = req.Barangay;
            addr.City = req.City;
            addr.Zip_Code = req.Zip_Code;
            addr.Updated_At = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return MapToResponse(addr);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var addr = await _db.Addresses.FindAsync(id);
            if (addr == null || addr.Deleted_At != null)
                return false;

            addr.Deleted_At = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }

        private AddressResponse MapToResponse(Address a)
        {
            return new AddressResponse
            {
                Address_Id = a.Address_Id,
                Unit_No = a.Unit_No,
                Street = a.Street,
                Barangay = a.Barangay,
                City = a.City,
                Zip_Code = a.Zip_Code
            };
        }
    }
}
