namespace Fahem.Dtos.OrdersDtos
{
    public class CreateOrderDto
    {
        public decimal Price { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
    }
}
