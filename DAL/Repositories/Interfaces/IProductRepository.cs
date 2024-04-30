using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IProductRepository
    {
        public Task<Product> GetProductDetail(int productId);
        public Task<List<Product>> GetAllProductsAsyncAdmin();
        public Task<List<Product>> GetProductByBrandId(int brandId);
        public Task<Product> GetProductDetailAsyncAdmin(int productId);
        public Task<List<Product>> GetProductSearchAsyncAdmin();
        public void UpdateProductAsync(Product product);

    }
}
