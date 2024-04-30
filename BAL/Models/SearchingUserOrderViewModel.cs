using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class SearchingUserOrderViewModel
    {
        public string? SearchValue { get; set; }
        public string? SortFullName { get; set; }
        public string? SortEmail { get; set; }
        public string? SortOrderDate { get; set; }
        public string? SortDeliveryMethod { get; set; }
        public string? SortTotalProduct { get; set; }
        public string? SortTotal { get; set; }
        public string? SortStatus { get; set; }
        public int? ItemPerPage { get; set; }
        public int? CurrentPage { get; set; }
        public int? totalPage { get; set; }
    }
}
