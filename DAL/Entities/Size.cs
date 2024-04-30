using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Size
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public virtual IEnumerable<DescribeProduct> DescribeProducts { get; set; }
    }
}
