using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class ProductDetailViewModel
    {
        public int Id{ get; set; }
        public string? Name { get; set; }
        public string? Composition { get; set; }
        public string? Description { get; set; }
        public virtual IEnumerable<ProductImageViewModel>? ProductImages { get; set; }
        public virtual IEnumerable<DescribeProductsViewModel>? DescribeProducts { get; set; }
    }
}
