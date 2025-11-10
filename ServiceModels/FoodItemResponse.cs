namespace Eatspress.ServiceModels
{
    public class FoodItemResponse
    {
        public int Item_Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Prep_Time { get; set; }
        public int Category_Id { get; set; }
        public float Price { get; set; }
        public byte[]? Image { get; set; }
        public bool IsDeleted { get; set; }
    }
}
