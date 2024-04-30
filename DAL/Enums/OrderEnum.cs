using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Enums
{
    public static class OrderEnum
    {
        public enum DeliveryStatus
        {
            CONFIRMING = 0,
            CONFIRMED = 1,
            DELIVERY = 2,
            DELIVERIED = 3,
            CANCELED = 4,
            RETURN = 5,
            EXCHANGE = 6
        }
        public enum Status
        {
            INACTIVE = 0,
            ACTIVE = 1
        }
    }
}
