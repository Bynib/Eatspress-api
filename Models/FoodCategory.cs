using System.Collections.Generic;

namespace Eatspress.Models
{
    public class FoodCategory
    {
        public int Category_Id { get; set; }
        public string Category_Type { get; set; }

        public ICollection<FoodItem> FoodItems { get; set; } = new List<FoodItem>();
    }
}
