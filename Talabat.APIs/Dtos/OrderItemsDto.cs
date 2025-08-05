namespace Talabat.APIs.Dtos
{
    public class OrderItemsDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

    }
}