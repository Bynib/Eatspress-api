using Eatspress.Models;
using Eatspress.ServiceModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eatspress.Interfaces
{
    public interface IAddressService
    {
        Task<AddressResponse> CreateAsync(int userId, AddressRequest req);
        Task<AddressResponse?> GetByIdAsync(int id);
        Task<IEnumerable<AddressResponse>> GetByUserAsync(int userId);
        Task<AddressResponse> UpdateAsync(UpdateAddressRequest req);
        Task<bool> DeleteAsync(int id);
    }
}
