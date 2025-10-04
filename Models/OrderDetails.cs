using System;

namespace Eatspress.Models
{
    public class OrderDetails
    {
        public int Order_Id { get; set; }
        public int Item_Id { get; set; }
        public int Quantity { get; set; }
        public Order Order { get; set; } = null;
        public FoodItem FoodItem { get; set; } = null;
    }
}
