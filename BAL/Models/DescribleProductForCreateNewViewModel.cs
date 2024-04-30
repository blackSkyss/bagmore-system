using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class DescribleProductForCreateNewViewModel
    {
      
       // public int ProductId { get; set; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public decimal OriginalPrice { get; set; }
        public int? ProviderId { get; set; }
    }
}
