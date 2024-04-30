using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface ICartProductRepository
    {
        public Task AddToCart(CartProduct cartProduct);
        public Task<CartProduct> GetProductFromCart(int cartId, int productId, string color, string size);
        public void UpdateProductFromCart(CartProduct cartProduct);
        public Task<List<CartProduct>> GetCartProducts(int cartId);
    }
}
