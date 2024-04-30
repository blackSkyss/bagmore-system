using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class ProductImage
    { 
        public int? Id { get; set; }
        public byte[]? Source { get; set; }
        public int? ProductId { get; set; }
        [JsonIgnore]
        public Product? Product { get; set; }
    }
}
