using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class CartProductViewModel
    {
        public int ProductId { get; set; }
        public string Color { get; set; }
        public int? Amount { get; set; }
        public string Size { get; set; }
    }
}
