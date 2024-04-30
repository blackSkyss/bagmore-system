using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Enums
{
    public static class ProductEnum
    {
        public enum Status
        {
            INACTIVE = 0,
            OUTOFSTOCK = 1,
            STOCKING = 2,
            SALE = 3
        }
    }
}
