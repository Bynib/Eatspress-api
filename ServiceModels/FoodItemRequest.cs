namespace Eatspress.ServiceModels
{
    public class FoodItemRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Prep_Time { get; set; }
        public int Category_Id { get; set; }
        public float Price { get; set; }
        public IFormFile? Image { get; set; }
    }
}
