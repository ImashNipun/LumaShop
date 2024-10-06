using LumaShopAPI.Entities;
using LumaShopAPI.Services.Database;
using MongoDB.Driver;

namespace LumaShopAPI.Services
{
    public class ShoppingCartService
    {
        private readonly IMongoCollection<ShoppingCart> _shoppingCarts;

        public ShoppingCartService(MongodbService mongodbService)
        {
            _shoppingCarts = mongodbService.Database.GetCollection<ShoppingCart>("shoppingCart");
        }

        // Create shopping cart
        public async Task<ShoppingCart> CreateShoppingCartAsync(ShoppingCart cart)
        {
            await _shoppingCarts.InsertOneAsync(cart);
            return cart;
        }

        // Get shopping cart by ID
        public async Task<ShoppingCart> GetShoppingCartByIdAsync(string id)
        {
            return await _shoppingCarts.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        // Update shopping cart
        public async Task<ShoppingCart> UpdateShoppingCartAsync(string id, ShoppingCart updatedCart)
        {
            var result = await _shoppingCarts.ReplaceOneAsync(c => c.Id == id, updatedCart);
            if (result.MatchedCount == 0)
            {
                return null;
            }
            return updatedCart;
        }

        // Delete shopping cart by ID
        public async Task<bool> DeleteShoppingCartAsync(string id)
        {
            var result = await _shoppingCarts.DeleteOneAsync(c => c.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
