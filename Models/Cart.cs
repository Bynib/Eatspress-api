using System;
using System.Collections.Generic;

namespace Eatspress.Models
{
    public class Cart
    {
        public int Item_Id { get; set; } 
        public int User_id { get; set; }
        public int Quantity { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime? Updated_At { get; set; }

        public User User { get; set; } = null;
        public FoodItem FoodItem { get; set; } = null;
    }
}
