using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class OrderProductsViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSize { get; set; }
        public string ProductColor { get; set; }
        public decimal Price { get; set; }
        public int Amout { get; set; }
        public List<ProductImageViewModel> productImages { get; set; }
    }
}
