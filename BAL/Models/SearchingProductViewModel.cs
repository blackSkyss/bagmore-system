using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class SearchingProductViewModel
    {
        public string? keySearchName { get; set; }
        public string? keySortName { get; set; }
        public string? keySortStatus { get; set; }
        public string? keySortOriginalPrice { get; set; }
        public string? keySortTTProduct { get; set; }
        public string? keySortDiscount { get; set; }
        public int? PAGE_NUMBER { get; set; }
        public int? PAGE_SIZE { get; set; }
    }
}
