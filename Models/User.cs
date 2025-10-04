using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Eatspress.Models
{
    public class User
    {
        [Key] public int User_Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phone_No { get; set; }
        public int Address_Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Role_Id { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime? Updated_At { get; set; }
        public DateTime? Deleted_At { get; set; }

        public ICollection<Address> Addresses { get; set; } = new List<Address>();
        public UserRole Role { get; set; } = null;
        public ICollection<Cart> Carts { get; set; } = new List<Cart>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
