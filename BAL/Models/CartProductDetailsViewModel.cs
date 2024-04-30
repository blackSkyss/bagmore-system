using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class CartProductDetailsViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public float Discount { get; set; }
        public int Amount { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public List<ProductImageViewModel> ProductImageViewModels { get; set; }
    }
}
