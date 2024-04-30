using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class CreatedProductViewModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Composition { get; set; }
        public float? Discount { get; set; }
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        public virtual IEnumerable<DescribleProductForCreateNewViewModel>? DescribeProducts { get; set; }
        public virtual IEnumerable<SupplierCreatedProductViewModel>? SuppliedProducts { get; set; }
        public virtual IEnumerable<ProductImageViewModel>? ProductImages { get; set; }
    }
}
