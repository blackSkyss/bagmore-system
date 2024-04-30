using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class SuppliedProduct
    {
        public int? Id { get; set; }
        public int? ProductId { get; set; }
        public Product? Product { get; set; }
        public int? SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
        public int? Amount { get; set; }
    }
}
