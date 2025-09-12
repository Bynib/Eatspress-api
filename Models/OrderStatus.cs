using System.Collections.Generic;

namespace Eatspress.Models
{
    public class OrderStatus
    {
        public int Status_Id { get; set; }
        public string Status_Type { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
