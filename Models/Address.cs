using System;
using System.Collections.Generic;

namespace Eatspress.Models
{
    public class Address
    {
        public int Address_Id { get; set; }
        public string Unit_No { get; set; }
        public string Street { get; set; }
        public string Barangay { get; set; }
        public string City { get; set; }
        public int Zip_Code { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime? Updated_At { get; set; }
        public DateTime? Deleted_At { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
