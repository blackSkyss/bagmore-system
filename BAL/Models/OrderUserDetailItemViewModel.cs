using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class OrderUserDetailItemViewModel
    {
        //Product
        public string? ProductName { get; set; }
        //DescribeProduct
        public float? Discount { get; set; }
        public decimal? Price { get; set; }
        //ProductOrder
        public int? Amount { get; set; }
    }
}
