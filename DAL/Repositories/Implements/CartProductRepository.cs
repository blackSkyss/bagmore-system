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
    public class CartProductRepository : RepositoryBase<CartProduct>, ICartProductRepository
    {
        private BagMoreDbContext _bagMoreDbContext;
        public CartProductRepository(BagMoreDbContext dbContext) : base(dbContext)
        {
            _bagMoreDbContext = dbContext;
        }

        public async Task AddToCart(CartProduct cartProduct)
        {
            try
            {
                await this._bagMoreDbContext.UserCarts.AddAsync(cartProduct);
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<CartProduct>> GetCartProducts(int cartId)
        {
            try
            {
                return await this._bagMoreDbContext.UserCarts
                            .Include(x => x.Product).ThenInclude(x => x.DescribeProducts).ThenInclude(x => x.Color)
                            .Include(x => x.Product).ThenInclude(x => x.DescribeProducts).ThenInclude(x => x.Size)
                            .Include(x => x.Product).ThenInclude(x => x.ProductImages)
                            .Where(x => x.CartId == cartId && x.Status == (int)CartProductEnum.Status.ACTIVE)
                            .ToListAsync();
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CartProduct> GetProductFromCart(int cartId, int productId, string color, string size)
        {
            try
            {
                return await this._bagMoreDbContext.UserCarts.FirstOrDefaultAsync(c => c.CartId == cartId && c.ProductId == productId 
                                                                                && c.Color.ToLower().Equals(color.ToLower()) 
                                                                                && c.Size.ToLower().Equals(size.ToLower()) 
                                                                                && c.Status == (int)CartProductEnum.Status.ACTIVE);
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateProductFromCart(CartProduct cartProduct)
        {
            try
            {
                this._bagMoreDbContext.Entry<CartProduct>(cartProduct).State = EntityState.Modified;
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
