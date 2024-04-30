using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class SearchingUserViewModel
    {
        public string? SearchValue { get; set; }
        public string? SortName { get; set; }
        public string? SortEmail { get; set; }
        public string? SortGender { get; set; }
        public string? SortBirthday { get; set; }
        public string? SortStatus { get; set; }
        public string? SortJoinDate { get; set; }
        public int? ItemsPerPage { get; set; }
        public int? CurrentPage { get; set; }

    }
}
