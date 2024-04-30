using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class AuthenticationRequest
    {
        [Required]
        public string IdToken { get; set; }
    }
}
