// This class is use to make a object for the request of updating order

using LumaShopAPI.LumaShopEnum;

namespace LumaShopAPI.DTOModals.Order
{
    public class UpdateOrderRequest
    {
        public OrderStatusEnum Status { get; set; }
    }
}
