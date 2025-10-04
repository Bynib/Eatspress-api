using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Eatspress.Models
{
    public class Order
    {
        [Key] public int Order_Id { get; set; }
        public int User_Id { get; set; }
        public int Status_Id { get; set; }
        public int Address_Id { get; set; }
        public string Instruction { get; set; }
        public int Estimated_Time { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime? Updated_At { get; set; }
        public User User { get; set; } = null;
        public Address Address { get; set; } = null;
        public OrderStatus Status { get; set; } = null;
        public ICollection<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();
    }
}
