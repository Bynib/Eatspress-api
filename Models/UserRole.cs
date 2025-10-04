using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Eatspress.Models
{
    public class UserRole
    {
        [Key] public int Role_Id { get; set; }
        public string Role_Title { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
