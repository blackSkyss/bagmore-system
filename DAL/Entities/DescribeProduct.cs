using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class DescribeProduct
    {
        public int? Id { get; set; }
        public int? ProductId { get; set; }
        public Product? Product { get; set; }
        public int? ColorId { get; set; }
        public Color? Color { get; set; }
        public int? SizeId { get; set; }
        public Size? Size { get; set; }
        public decimal? Price { get; set; }
        public int? Amount { get; set; }
        public decimal? OriginalPrice { get; set; }
        public int? providerId { get; set; }
    }
}
