using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Order
    {
        public int? Id { get; set; }
        public int? DeliveryStatus { get; set; }
        public DateTime? OrderedDate { get; set; }
        public string? ShippingAddress { get; set; }
        public int? Status { get; set; }

        public int? DeliveryMethodId { get; set; }
        public DeliveryMethod? DeliveryMethod { get; set; }
        public decimal DeliveryMethodPrice { get; set; }
        public int? UserID { get; set; }
        public User? User { get; set; }

        public virtual IEnumerable<ProductOrder>? ProductOrders { get; set; }
    }
}
