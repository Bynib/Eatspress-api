using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Eatspress.Models
{
    public class OrderStatus
    {
        [Key] public int Status_Id { get; set; }
        public string Status_Type { get; set; }
    
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
