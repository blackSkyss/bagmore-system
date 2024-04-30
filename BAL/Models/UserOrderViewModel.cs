using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class UserOrderViewModel
    {
        public int OrderID { get; set; }
        public string UserFullName { get; set; }
        public string UserEmail { get; set; }
        public DateTime OrderedDate { get; set; }
        public string DeliveryMethod { get; set; }
        public int TotalProduct { get; set; }
        public decimal TotalPrice { get; set; }
        public int DeliveryStatus { get; set; }
    }
}
