using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class BrandCreatedViewModel
    {
        public string? Name { get; set; }
        public byte[]? Logo { get; set; }
        public int? Status { get; set; }
    }
}
