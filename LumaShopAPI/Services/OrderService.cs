/*
 * This class handles operations related to orders in the LumaShop API.
 * It provides methods for creating, updating, and retrieving orders 
 * from the MongoDB database.
 */


using LumaShopAPI.DTOModals.Order;
using LumaShopAPI.Entities;
using LumaShopAPI.LumaShopEnum;
using LumaShopAPI.Services.Database;
using MongoDB.Driver;

namespace LumaShopAPI.Services
{
    public class OrderService
    {
        private readonly IMongoCollection<Order> _orders;

        public OrderService(MongodbService mongodbService)
        {
            _orders = mongodbService.Database.GetCollection<Order>("orders");
        }

        // Create a new order in the database
        public async Task<Order> CreateOrderAsync(Order order)
        {
            await _orders.InsertOneAsync(order);
            return order;
        }

        // Update an existing order in the database
        public async Task<Order> UpdateOrderAsync(string orderId, Order order)
        {
            await _orders.ReplaceOneAsync(o => o.Id == orderId, order);
            return order;
        }

        // Get an order by its ID
        public async Task<Order> GetOrderByIdAsync(string orderId)
        {
            return await _orders.Find(o => o.Id == orderId).FirstOrDefaultAsync();
        }

        // Get all orders from the database
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _orders.Find(_ => true).ToListAsync();
        }
    }
}
