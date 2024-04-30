using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class SearchingProductFromUserViewModel
    {
        public string? sortby { get; set; }
        public List<int>? filterbycategory { get; set; }
        public List<int>? filterbycolor { get; set; }
        public List<int>? filterbysize { get; set; }
    }
}
