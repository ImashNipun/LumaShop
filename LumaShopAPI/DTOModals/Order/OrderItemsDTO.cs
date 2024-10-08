// This use to create  a order object for create order and update order request

namespace LumaShopAPI.DTOModals.Order
{
    public class OrderItemsDTO
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
