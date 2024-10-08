/*
 * This service class provides methods to interact with the ProductListing collection
 * in the MongoDB database. It includes functionalities for creating, retrieving, 
 * updating, and deleting product listings, as well as checking if a listing is active.
 */

using LumaShopAPI.Entities;
using LumaShopAPI.Services.Database;
using MongoDB.Driver;

namespace LumaShopAPI.Services
{
    public class ProductListingService
    {
        private readonly IMongoCollection<ProductListing> _productListings;

        public ProductListingService(MongodbService mongodbService)
        {
            _productListings = mongodbService.Database.GetCollection<ProductListing>("productListings");
        }

        // Create a new ProductListing in the database
        public async Task<ProductListing> CreateAsync(ProductListing productListing)
        {
            await _productListings.InsertOneAsync(productListing);
            return productListing;
        }

        // Get all ProductListings in the database
        public async Task<List<ProductListing>> GetAllAsync()
        {
            return await _productListings.Find(_ => true).ToListAsync();
        }

        // Get a single ProductListing by Id in the database
        public async Task<ProductListing> GetByIdAsync(string id)
        {
            return await _productListings.Find(listing => listing.Id == id).FirstOrDefaultAsync();
        }

        // Update an existing ProductListing in the database
        public async Task UpdateAsync(string id, ProductListing productListing)
        {
            productListing.UpdatedAt = DateTime.UtcNow;
            await _productListings.ReplaceOneAsync(listing => listing.Id == id, productListing);
        }

        // Delete a ProductListing in the database
        public async Task DeleteAsync(string id)
        {
            await _productListings.DeleteOneAsync(listing => listing.Id == id);
        }

        public async Task<bool> IsListingActiveAsync(string id)
        {
            var listing = await _productListings.Find(listing => listing.Id == id).FirstOrDefaultAsync();
            return listing?.IsActive ?? false;
        }
    }
}
