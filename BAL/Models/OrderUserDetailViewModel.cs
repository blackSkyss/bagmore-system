using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class OrderUserDetailViewModel
    {
        //User
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        // Order
        public int? OrderId { get; set; }
        public int? DeliveryStatus { get; set; }
        public string? DeliveryMethod { get; set; }
        public DateTime OrderedDate { get; set; }
        public string? ShippingAddress { get; set; }
        public List<OrderUserDetailItemViewModel>? orderUserDetailItemViewModels { get; set; }
        //DeliveryMethod
        public decimal ShippingPrice { get; set; }
        public decimal Total { get; set; }
        public decimal SubTotal { get; set; }




    }
}
