using LumaShopAPI.DTOModals.Order;
using LumaShopAPI.Entities;
using LumaShopAPI.LumaShopEnum;
using MongoDB.Driver;

namespace LumaShopAPI.Services
{
    public class OrderService
    {
        private readonly IMongoCollection<Order> _orders;

        public OrderService(IMongoDatabase database)
        {
            _orders = database.GetCollection<Order>("orders");
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            await _orders.InsertOneAsync(order);
            return order;
        }

        public async Task<Order> UpdateOrderAsync(string orderId, Order order)
        {
            await _orders.ReplaceOneAsync(o => o.Id == orderId, order);
            return order;
        }

        public async Task<Order> GetOrderByIdAsync(string orderId)
        {
            return await _orders.Find(o => o.Id == orderId).FirstOrDefaultAsync();
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _orders.Find(_ => true).ToListAsync();
        }
    }
}
