using DAL.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class UserAccountViewModel
    {
        public string Email { get; set; }
        public string? Password { get; set; }
        public bool? Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDay { get; set; }
        public string? Phone { get; set; }
        public string? FirstAddress { get; set; }
        public string? SecondAddress { get; set; }
        public IFormFile? Image { get; set; }
    }
}
