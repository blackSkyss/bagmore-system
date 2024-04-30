using DAL.Entities;
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
    public class CartRepository : RepositoryBase<Cart>, ICartRepository
    {
        private BagMoreDbContext _bagMoreDbContext;
        public CartRepository(BagMoreDbContext dbContext) : base(dbContext)
        {
            _bagMoreDbContext = dbContext;
        }

        public async Task CreateUserCart(Cart cart)
        {
            try
            {
                await this._bagMoreDbContext.Carts.AddAsync(cart);
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Cart> GetUserCart(int userId)
        {
            try
            {
                return await this._bagMoreDbContext.Carts.FirstOrDefaultAsync(x => x.UserId == userId);
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
