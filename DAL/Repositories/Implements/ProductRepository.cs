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
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        private BagMoreDbContext _bagMoreDbContext;
        public ProductRepository(BagMoreDbContext dbContext) : base(dbContext)
        {
            _bagMoreDbContext = dbContext;
        }

        public async Task<Product> GetProductDetail(int productId)
        {
            try
            {
                Product product;
                return product =
                _bagMoreDbContext.Products.
                Include(p => p.DescribeProducts).ThenInclude(p => p.Color)
                 .Include(p => p.DescribeProducts).ThenInclude(p => p.Size)
                 .Include(p => p.ProductImages)
                 .SingleOrDefault(x => x.Id == productId && x.Status == (int)ProductEnum.Status.STOCKING || x.Status == (int)ProductEnum.Status.SALE);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Product>> GetAllProductsAsyncAdmin()
        {
            try
            {
                List<Product> products = await _bagMoreDbContext.Products.Include(p => p.Category)
                    .Include(p => p.ProductImages).
                    Include(p => p.Brand).ToListAsync();
                return products;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Product> GetProductDetailAsyncAdmin(int productId)
        {
            try
            {
                Product product = await _bagMoreDbContext.Products
                    .Include(p => p.Category)
                    .Include(p => p.ProductImages)
                    .Include(p => p.Brand)
                    .Include(p => p.DescribeProducts).ThenInclude(dp => dp.Color)
                    .Include(p => p.DescribeProducts).ThenInclude(dp => dp.Size)
                    .Include(p => p.SuppliedProducts).ThenInclude(sp => sp.Supplier)
                    .SingleOrDefaultAsync(p => p.Id == productId);
                ;
                return product;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Product>> GetProductSearchAsyncAdmin()
        {
            try
            {
                List<Product> product = await _bagMoreDbContext.Products.Include(p => p.Category)
                    .Include(p => p.ProductImages)
                    .Include(p => p.DescribeProducts)
                    .Include(p => p.Brand).ToListAsync();
                ;
                return product;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Product>> GetProductByBrandId(int brandId)
        {
            List<Product> product = await _bagMoreDbContext.Products.Where(p => p.BrandId == brandId).ToListAsync();
            return product;
        }

        public void UpdateProductAsync(Product product)
        {
            try
            {
                this._bagMoreDbContext.Entry<Product>(product).State = EntityState.Modified;
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
