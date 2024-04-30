using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool? Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDay { get; set; }
        public string Phone { get; set; }
        public string FirstAddress { get; set; }
        public string SecondAddress { get; set; }
        public byte[] Avatar { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }

        public Cart Cart { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }

        public virtual IEnumerable<ShippingAddress> ShippingAddresses { get; set; }
        public virtual IEnumerable<UserToken> UserTokens { get; set; }
        public virtual IEnumerable<Order> Orders { get; set; }
        public virtual IEnumerable<WishList> WishLists { get; set; }
    }
}
