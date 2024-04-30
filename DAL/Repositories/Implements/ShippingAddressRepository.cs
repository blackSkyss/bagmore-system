using DAL.Entities;
using DAL.Enums;
using DAL.Infrastructure;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implements
{
    public class ShippingAddressRepository : RepositoryBase<ShippingAddress>, IShippingAddressRepository
    {
        private BagMoreDbContext _bagMoreDbContext;
        List<ShippingAddress> _shippingAddresses;
        public ShippingAddressRepository(BagMoreDbContext dbContext) : base(dbContext)
        {
            _bagMoreDbContext = dbContext;
        }

        public async Task<List<ShippingAddress>> GetShippingAddressAsync(int userId)
        {
            
            try
            {
                 _shippingAddresses = await _bagMoreDbContext.ShippingAddresses.Where(s => s.IdUser.Equals(userId)).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Can not get all shipping address!!!");
            }
            return _shippingAddresses;
        }

        public async Task<ShippingAddress> GetShippingAddressByIdAsync(int shippingAddressId, int userId)
        {
            try
            {
                return await this._bagMoreDbContext.ShippingAddresses.FirstOrDefaultAsync(x => x.Id == shippingAddressId 
                && x.IdUser == userId && x.Status == (int)ShippingAddressEnum.Status.ACTIVE);
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
