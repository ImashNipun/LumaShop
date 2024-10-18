/*
 * This class provides methods to manage products in the LumaShop database,
 * including retrieving, creating, updating, and deleting products,
 * as well as managing stock levels.
 */

using LumaShopAPI.Entities;
using LumaShopAPI.Services.Database;
using MongoDB.Driver;

namespace LumaShopAPI.Services
{
    public class ProductService
    {
        private readonly IMongoCollection<Product> _products;
        private readonly ProductListingService _productListingService;

        public ProductService(MongodbService mongodbService, ProductListingService productListingService)
        {
            _products = mongodbService.Database.GetCollection<Product>("products");
            _productListingService = productListingService;
        }

        // Get all products from the database
        public async Task<List<Product>> GetAllAsync()
        {
            var result = await _products.Find(product => true).ToListAsync();
            var activeProducts = new List<Product>();
            foreach (var product in result)
            {
                if (await _productListingService.IsListingActiveAsync(product.ListingId))
                {
                    activeProducts.Add(product);
                }
            }

            return activeProducts;

        }


        // Get a product by its ID from the database
        public async Task<Product> GetByIdAsync(string id)
        {
            var result = await _products.Find(product => product.Id == id).FirstOrDefaultAsync();
            if (result != null && await _productListingService.IsListingActiveAsync(result.ListingId))
            {
                return result;
            }

            return null;
        }


        // Create a new product in the database
        public async Task<Product> CreateAsync(Product product)
        {
            await _products.InsertOneAsync(product);
            return product;
        }

        // Update an existing product in the database
        public async Task UpdateAsync(string id, Product product)
        {
            product.UpdatedAt = DateTime.UtcNow;
            await _products.ReplaceOneAsync(listing => listing.Id == id, product);
        }

        // Delete a product from the database
        public async Task DeleteAsync(string id)
        {
            await _products.DeleteOneAsync(product => product.Id == id);
        }

        // Deduct stock quantity of a product by a specified amount
        public async Task<bool> DeductStockAsync(string productId, int quantity)
        {
            var filter = Builders<Product>.Filter.And(Builders<Product>.Filter.Eq(p => p.Id, productId),Builders<Product>.Filter.Gte(p => p.StockQuantity, quantity));
            var update = Builders<Product>.Update.Inc(p => p.StockQuantity, -quantity);

            var result = await _products.UpdateOneAsync(filter, update);

            if (result.ModifiedCount == 0)
            {
                return false;
            }

            return true;
        }


        // Get all products from the database
        public async Task<List<Product>> GetAllProductsByVendorIdAsync(string vendorId)
        {
            return await _products.Find(product => product.VendorId == vendorId).ToListAsync();
        }

        public async Task<List<Product>> GetAllProductAsync()
        {
            return await _products.Find(product => true).ToListAsync();
        }

    }
}
