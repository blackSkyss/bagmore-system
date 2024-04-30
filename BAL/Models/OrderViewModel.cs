using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public int DeliveryStatus { get; set; }
        public DateTime OrderedDate { get; set; }
        public string ShippingAddress { get; set; }
        public int Status { get; set; }
        public string DeliveryMethod { get; set; }
        public List<OrderProductsViewModel> products { get; set; }


    }
}
