using Eatspress.Models;
namespace YourApp.ServiceModels
{
    public class CartRequest
    {
        public int User_Id { get; set; }
        public int Item_Id { get; set; }
        public int Quantity { get; set; }
    }
}

