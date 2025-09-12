using System.Collections.Generic;

namespace Eatspress.Models
{
    public class UserRole
    {
        public int Role_Id { get; set; }
        public string Role_Title { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
