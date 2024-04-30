using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IShippingAddressRepository
    {
        Task<List<ShippingAddress>> GetShippingAddressAsync(int userId);
        public Task<ShippingAddress> GetShippingAddressByIdAsync(int shippingAddressId, int userId);
    }
}
