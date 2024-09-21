namespace LumaShopAPI.DTOModals.Order
{
    public class CreateOrderRequest
    {
        public string CustomerId { get; set; }
        public List<OrderItemsDTO> Items { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
