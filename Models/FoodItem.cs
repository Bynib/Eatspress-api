using System;
using System.Collections.Generic;

namespace Eatspress.Models
{
    public class FoodItem
    {
        public int Item_Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Prep_Time { get; set; }
        public int Category_Id { get; set; }
        public float Price { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime? Updated_At { get; set; }
        public DateTime? Deleted_At { get; set; }

        public FoodCategory Category { get; set; } = null;
        public ICollection<CartDetails> CartDetails { get; set; } = new List<CartDetails>();
    }
}
