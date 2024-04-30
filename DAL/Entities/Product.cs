using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Product
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Composition { get; set; }
        public string? Description { get; set; }
        public float? Discount { get; set; }
        public int? TotalProduct { get; set; }
        public int? Status { get; set; }

        public int? BrandId { get; set; }
        public Brand? Brand { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public virtual IEnumerable<ProductImage>? ProductImages { get; set; }
        public virtual IEnumerable<ProductOrder>? ProductOrders { get; set; }
        public virtual IEnumerable<DescribeProduct>? DescribeProducts { get; set; }
        public virtual IEnumerable<SuppliedProduct>? SuppliedProducts { get; set; }
        public virtual IEnumerable<CartProduct>? CartProducts { get; set; }
        public virtual IEnumerable<WishList>? WishLists { get; set; }

    }
}
