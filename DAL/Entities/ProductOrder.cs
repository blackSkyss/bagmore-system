using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class ProductOrder
    {
        public int? Id { get; set; }
        public int? IdProduct { get; set; }
        public Product? Product { get; set; }
        public int? IdOrder { get; set; }
        public Order? Order { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public decimal? Price { get; set; }
        public int? Amount { get; set; }
    }
}
