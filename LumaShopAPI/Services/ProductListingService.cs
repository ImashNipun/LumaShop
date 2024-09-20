using LumaShopAPI.Entities;
using MongoDB.Driver;

namespace LumaShopAPI.Services
{
    public class ProductListingService
    {
        private readonly IMongoCollection<ProductListing> _productListings;

        public ProductListingService(IMongoDatabase database)
        {
            _productListings = database.GetCollection<ProductListing>("ProductListings");
        }

        // Create new ProductListing
        public async Task<ProductListing> CreateAsync(ProductListing productListing)
        {
            await _productListings.InsertOneAsync(productListing);
            return productListing;
        }

        // Get all ProductListings
        public async Task<List<ProductListing>> GetAllAsync()
        {
            return await _productListings.Find(_ => true).ToListAsync();
        }

        // Get a single ProductListing by Id
        public async Task<ProductListing> GetByIdAsync(string id)
        {
            return await _productListings.Find(listing => listing.Id == id).FirstOrDefaultAsync();
        }

        // Update an existing ProductListing
        public async Task UpdateAsync(string id, ProductListing productListing)
        {
            productListing.UpdatedAt = DateTime.UtcNow;
            await _productListings.ReplaceOneAsync(listing => listing.Id == id, productListing);
        }

        // Delete a ProductListing
        public async Task DeleteAsync(string id)
        {
            await _productListings.DeleteOneAsync(listing => listing.Id == id);
        }
    }
}
