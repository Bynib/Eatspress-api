namespace Eatspress.ServiceModels
{
    public class OrderResponse
    {
        public int Order_Id { get; set; }
        public int User_Id { get; set; }
        public int Address_Id { get; set; }
        public int Status_Id { get; set; }
        public string? Instruction { get; set; }
        public int Estimated_Time { get; set; }
        public DateTime Created_At { get; set; }
        public ICollection<OrderDetailsResponse> Details { get; set; } = new List<OrderDetailsResponse>();
    }
}
