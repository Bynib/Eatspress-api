using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Eatspress.Models
{
    public class Address
    {
        [Key] public int Address_Id { get; set; }
        public int User_Id { get; set; }
        public string Unit_No { get; set; }
        public string Street { get; set; }
        public string Barangay { get; set; }
        public string City { get; set; }
        public int Zip_Code { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime? Updated_At { get; set; }
        public DateTime? Deleted_At { get; set; }

        public User User { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
