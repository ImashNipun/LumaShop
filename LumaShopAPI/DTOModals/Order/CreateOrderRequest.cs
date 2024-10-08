// This class use to make a object for the request of creating order

namespace LumaShopAPI.DTOModals.Order
{
    public class CreateOrderRequest
    {
        public string CustomerId { get; set; }
        public List<OrderItemsDTO> Items { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
