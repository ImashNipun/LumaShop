/*
 * This class provides methods for managing vendor ratings in the LumaShop application.
 * It includes functionalities to create, read, update, and delete vendor ratings 
 * stored in a MongoDB database.
 */

using LumaShopAPI.Entities;
using LumaShopAPI.Services.Database;
using MongoDB.Driver;

namespace LumaShopAPI.Services
{
    public class VendorRatingService
    {
        private readonly IMongoCollection<VendorRatings> _ratings;
        public VendorRatingService(MongodbService mongodbService)
        {
            _ratings = mongodbService.Database.GetCollection<VendorRatings>("vendorRatings");
        }

        // Get all vendor ratings from the database
        public async Task<List<VendorRatings>> GetAllAsync()
        {
            return await _ratings.Find(rating => true).ToListAsync();
        }

        // Get a vendor rating by Id from the database
        public async Task<VendorRatings> GetByIdAsync(string id)
        {
            return await _ratings.Find(vr => vr.Id == id).FirstOrDefaultAsync();
        }

        // Create a new vendor rating in the database
        public async Task<VendorRatings> CreateAsync(VendorRatings vendorRating)
        {
            await _ratings.InsertOneAsync(vendorRating);
            return vendorRating;
        }

        // Update a vendor rating in the database
        public async Task UpdateAsync(string id, VendorRatings vendorRating)
        {
            await _ratings.ReplaceOneAsync(vr => vr.Id == id, vendorRating);
        }

        // Delete a vendor rating from the database
        public async Task DeleteAsync(string id)
        {
            await _ratings.DeleteOneAsync(vr => vr.Id == id);
        }
    }
}
