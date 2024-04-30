using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class SearchingSupplierViewModel
    {
        public string? keysearch { get; set; }
        public string? keysortname { get; set; }
        public string? keysortemail { get; set; }
        public string? keysortphone { get; set; }
        public string? keySortTTProduct { get; set; }
        public string? keysortstatus { get; set; }
        public int? PAGE_SIZE { get; set; }
        public int? PAGE_NUMBER { get; set; }
    }
}
