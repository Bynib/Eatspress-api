using System;
using System.Collections.Generic;

namespace Eatspress.Models
{
    public class Cart
    {
        public int Item_Id { get; set; }  // As per ERD, but should be Cart_Id ideally
        public int Customer_Id { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime? Updated_At { get; set; }
        public DateTime? Deleted_At { get; set; }

        public User Customer { get; set; } = null;
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
