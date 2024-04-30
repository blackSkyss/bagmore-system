using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IDescribeProductRepository
    {
        public Task<DescribeProduct> GetDescribeProductAsync(int productId, string colorName, string sizeName);
        public Task<List<DescribeProduct>> GetDescribeProductsAsync(int productId);
        public void UpdateDescribeProduct(DescribeProduct describeProduct);
    }
}
