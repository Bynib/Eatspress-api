namespace Eatspress.ServiceModels
{
    public class OrderRequest
    {
        public int Address_Id { get; set; }
        public int User_Id { get; set; }
        public string Instruction { get; set; }
        public int Estimated_Time { get; set; }
        public ICollection<OrderDetailsRequest> Details { get; set; } = new List<OrderDetailsRequest>();
    }
}
