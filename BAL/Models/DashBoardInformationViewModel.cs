using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class DashBoardInformationViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalUserOrders { get; set; }
        public int TotalSuppliers { get; set; }
        public int TotalBrands { get; set; }
        public int TotalCategories { get; set; }
        public int TotalDeliveryMethods { get; set; }
        public List<int> Years { get; set; }
        public List<OrderSoldViewModel> OrderSolds { get; set; }
        public List<NumberOfProfitViewModel> NumberOfProfits { get; set; }

    }
}
