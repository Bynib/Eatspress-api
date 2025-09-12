using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eatspress.Models;

namespace Eatspress.Services
{
    public class AddressService
    {
        private readonly EatspressContext _context;

        public AddressService(EatspressContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Address>> GetAllAsync()
        {
            return await _context.Addresses.ToListAsync();
        }

        public async Task<Address> GetByIdAsync(int id)
        {
            return await _context.Addresses.FindAsync(id);
        }

        public async Task<Address> CreateAsync(Address address)
        {
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<Address> UpdateAsync(int id, Address address)
        {
            var existing = await _context.Addresses.FindAsync(id);
            if (existing == null) return null;

            existing.Unit_No = address.Unit_No;
            existing.Street = address.Street;
            existing.Barangay = address.Barangay;
            existing.City = address.City;
            existing.Zip_Code = address.Zip_Code;
            existing.Updated_At = System.DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address == null) return false;

            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
