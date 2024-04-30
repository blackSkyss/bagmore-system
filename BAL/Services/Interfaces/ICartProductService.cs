using BAL.Models;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Interfaces
{
    public interface ICartProductService
    {
        public Task AddToCart(CartProductViewModel cartProduct, string username);
        public Task RemoveFromCart(CartProductViewModel cartProduct, string username);
        public Task<List<CartProductDetailsViewModel>> GetCartProducts(string username);
        public Task Checkout(CheckoutViewModel checkoutViewModel, string username);
    }
}
