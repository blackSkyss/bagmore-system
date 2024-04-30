using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class UploadFile
    {
      public IFormFile image { get; set; }

        public string name { get; set; }
    }
}
