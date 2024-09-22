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

        public async Task<List<VendorRatings>> GetAllAsync()
        {
            return await _ratings.Find(rating => true).ToListAsync();
        }

        public async Task<VendorRatings> GetByIdAsync(string id)
        {
            return await _ratings.Find(vr => vr.Id == id).FirstOrDefaultAsync();
        }

        public async Task<VendorRatings> CreateAsync(VendorRatings vendorRating)
        {
            await _ratings.InsertOneAsync(vendorRating);
            return vendorRating;
        }

        public async Task UpdateAsync(string id, VendorRatings vendorRating)
        {
            await _ratings.ReplaceOneAsync(vr => vr.Id == id, vendorRating);
        }

        public async Task DeleteAsync(string id)
        {
            await _ratings.DeleteOneAsync(vr => vr.Id == id);
        }
    }
}
