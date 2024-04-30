using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class DescribeProductsViewModel
    {
        public ColorViewModel Color { get; set; }
        public SizeViewModel Size { get; set; }
        public decimal Price { get; set; }
    }
}
