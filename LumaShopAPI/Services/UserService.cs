using LumaShopAPI.Entities;
using LumaShopAPI.Services.Database;
using MongoDB.Driver;
using LumaShopAPI.LumaShopEnum;

namespace LumaShopAPI.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(MongodbService mongodbService)
        {
            _users = mongodbService.Database.GetCollection<User>("users");  // Specify your collection name here
        }

        // Get all users
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _users.Find(u => true).ToListAsync();
        }

        // Get a user by Id
        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _users.Find(u => u.Id == Guid.Parse(id)).FirstOrDefaultAsync();
        }

        // Get a user by Id
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _users.Find(u => u.Email == email & u.Status == UserStatusEnum.ACTIVE).FirstOrDefaultAsync();
        }

        // Update a user
        public async Task UpdateUserAsync(string id, User updatedUser)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, Guid.Parse(id));
            await _users.ReplaceOneAsync(filter, updatedUser);
        }

        // Delete a user
        public async Task DeleteUserAsync(string id)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, Guid.Parse(id));
            await _users.DeleteOneAsync(filter);
        }

        //Check if already user exist with the email
        public bool CheckIfEmailExists(string email)
        {
            return _users.Find(u => u.Email == email).Any();
        }
    }
}
