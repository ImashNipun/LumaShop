/*
 * This class represents a user entity within the LumaShop API.
 * It inherits from MongoIdentityUser and defines additional 
 * properties for user-specific information including first name, 
 * last name, company name, description, role, and status.
 */

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using LumaShopAPI.LumaShopEnum;
using AspNetCore.Identity.MongoDbCore.Models;

namespace LumaShopAPI.Entities
{
    public class User: MongoIdentityUser<Guid>
    {

        [BsonRequired, BsonElement("firstName")]
        public string FirstName { get; set; } = string.Empty; 

        [BsonRequired, BsonElement("lastName")]
        public string LastName { get; set; } = string.Empty;

        [BsonElement("companyName")]
        public string? CompanyName { get; set; }

        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonRequired, BsonElement("role"), BsonRepresentation(BsonType.Int32)]
        public UserRoleEnum Role { get; set; }

        [BsonRequired, BsonElement("status"), BsonRepresentation(BsonType.Int32)]
        public UserStatusEnum Status { get; set; } = UserStatusEnum.PENDING;
    }
}




