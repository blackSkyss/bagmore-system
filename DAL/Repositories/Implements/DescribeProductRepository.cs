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
    public class DescribeProductRepository : RepositoryBase<DescribeProduct>, IDescribeProductRepository
    {
        private BagMoreDbContext _bagMoreDbContext;
        public DescribeProductRepository(BagMoreDbContext dbContext) : base(dbContext)
        {
            _bagMoreDbContext = dbContext;
        }

        public async Task<DescribeProduct> GetDescribeProductAsync(int productId, string colorName, string sizeName)
        {
            try
            {
               return await this._bagMoreDbContext.DescribeProducts.FirstOrDefaultAsync(x => x.ProductId == productId
                                                    && x.Color.Name.ToLower().Equals(colorName.ToLower())
                                                    && x.Size.Name.ToLower().Equals(sizeName.ToLower()));
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<DescribeProduct>> GetDescribeProductsAsync(int productId)
        {
            try
            {
                return await this._bagMoreDbContext.DescribeProducts.Where(x => x.ProductId == productId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateDescribeProduct(DescribeProduct describeProduct)
        {
            try
            {
                this._bagMoreDbContext.Entry<DescribeProduct>(describeProduct).State = EntityState.Modified;
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
