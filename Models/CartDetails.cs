using System;

namespace Eatspress.Models
{
    public class CartDetails
    {
        public int Order_Id { get; set; }
        public int Item_Id { get; set; }
        public string Instruction { get; set; }
        public int Quantity { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime? Updated_At { get; set; }
        public DateTime? Deleted_At { get; set; }

        public Order Order { get; set; } = null;
        public FoodItem Item { get; set; } = null;
    }
}
