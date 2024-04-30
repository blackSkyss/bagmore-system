using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class ProductViewModelAdmin
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Composition { get; set; }
        public string? Description { get; set; }
        public float? Discount { get; set; }
        public int? TotalProduct { get; set; }
        public int? Status { get; set; }
        public decimal OriginalPrice { get; set; }
        public BrandNameViewModel? Brand { get; set; }
        public CategoryViewModel? Category { get; set; }
        public virtual IEnumerable<ProductImageViewModel>? ProductImages { get; set; }
        public List<SuppliedProductViewModel>? SuppliedProducts { get; set; } 
        public List<AdminDescribeProductViewModel>? DescribeProducts { get; set; }
    }
}
