
using BAL.Models;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Interfaces
{
    public interface IProductService
    {
       public Task<ProductDetailViewModel> GetProductInformation(int productId);
        public Task<bool> CreateProductAsync(CreatedProductViewModel productViewModel);
        Task<bool> UpdateProductAsync(int productId , CreatedProductViewModel productViewModel);
        public Task<List<ProductViewModelAdmin>> GetAllProductsAsyncAdmin();
        public Task<bool> DeleteProduct(int productId);
        public Task<ProductViewModelAdmin> GetProductDetailAsyncAdmin(int productId);
        public Task<Tuple<List<ProductViewModelAdmin>, int, int?, int?>> GetProductsAsync(string? keySearchName,
                                                                string? keySortStatus, string? keySortOriginalPrice, string? keySortName,
                                                                string? keySortDiscount, string? keySortTTProduct,
                                                                int? PAGE_NUMBER, int? PAGE_SIZE);
        Task<List<ProductViewModel>> GetProductsAsync(string? sortby, List<int>? filterbycategory, List<int>? filterbycolor,List<int>? filterbysize);
    }
}
