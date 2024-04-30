using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class BrandViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public byte[]? Logo { get; set; }
        public int? Status { get; set; }
        public int? TotalProducts { get; set; }

        
    }
}
