using System;
using System.Collections.Generic;

namespace Eatspress.Models
{
    public class Order
    {
        public int Order_Id { get; set; }
        public int Status_Id { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime? Updated_At { get; set; }
        public DateTime? Deleted_At { get; set; }

        public OrderStatus Status { get; set; } = null;
        public ICollection<CartDetails> CartDetails { get; set; } = new List<CartDetails>();
    }
}
