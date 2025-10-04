using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Eatspress.Models
{
    public class FoodCategory
    {
        [Key] public int Category_Id { get; set; }
        public string Category_Type { get; set; }

        public ICollection<FoodItem> FoodItems { get; set; } = new List<FoodItem>();
    }
}
