using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Eatspress.Models
{
    public class FoodItem
    {
        [Key] public int Item_Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Prep_Time { get; set; }
        public int Category_Id { get; set; }
        public float Price { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime? Updated_At { get; set; }
        public DateTime? Deleted_At { get; set; }

        public FoodCategory Category { get; set; } = null;
        public ICollection<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();
        public ICollection<Cart> Carts { get; set; } = new List<Cart>();
    }
}
