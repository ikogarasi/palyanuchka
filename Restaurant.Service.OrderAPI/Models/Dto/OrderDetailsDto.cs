namespace Restaurant.Service.OrderAPI.Models.Dto
{
    public class OrderDetailsDto
    {
        public int OrderDetailsId { get; set; }
        public int OrderHeaderId { get; set; }
        public virtual OrderHeaderDto OrderHeader { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public string ProductName { get; set; }
        public string Price { get; set; }
    }
}
