//using MongoDB.Bson.Serialization.Attributes;
//using MongoDB.Bson;
//using LumaShopAPI.LumaShopEnum;

//namespace LumaShopAPI.Entities
//{
//    public class User
//    {
//        [BsonId]
//        [BsonRepresentation(BsonType.ObjectId)]
//        public string? Id { get; set; }

//        [BsonRequired, BsonElement("firstName")]
//        public string FirstName { get; set; } = string.Empty;  // Default to empty string if not set

//        [BsonRequired, BsonElement("lastName")]
//        public string LastName { get; set; } = string.Empty;   // Default to empty string if not set

//        [BsonRequired, BsonElement("emailAddress"), BsonRepresentation(BsonType.String)]
//        public string EmailAddress { get; set; }

//        [BsonElement("dateOfBirth"), BsonRepresentation(BsonType.DateTime)]
//        public DateTime? DateOfBirth { get; set; }  // Optional

//        [BsonRequired, BsonElement("password")]
//        public string Password { get; set; }

//        [BsonRequired, BsonElement("role"), BsonRepresentation(BsonType.Int32)]
//        public UserRoleEnum Role { get; set; }

//        [BsonRequired, BsonElement("status"), BsonRepresentation(BsonType.Int32)]
//        public UserStatusEnum Status { get; set; } = UserStatusEnum.PENDING;   // Default to PENDING

//        [BsonElement("isArchived"), BsonRepresentation(BsonType.Boolean)]
//        public bool IsArchived { get; set; } = false;   // Default to false

//        [BsonRequired, BsonElement("updatedAt"), BsonRepresentation(BsonType.DateTime)]
//        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;   // Auto-set timestamps

//        [BsonRequired, BsonElement("createdAt"), BsonRepresentation(BsonType.DateTime)]
//        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;   // Auto-set timestamps

//        [BsonElement("lastLogin"), BsonRepresentation(BsonType.DateTime)]
//        public DateTime? LastLogin { get; set; }  // Optional
//    }
//}



using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using LumaShopAPI.LumaShopEnum;
using AspNetCore.Identity.MongoDbCore.Models;

namespace LumaShopAPI.Entities
{
    public class User: MongoIdentityUser<Guid>
    {

        [BsonRequired, BsonElement("firstName")]
        public string FirstName { get; set; } = string.Empty;  // Default to empty string if not set

        [BsonRequired, BsonElement("lastName")]
        public string LastName { get; set; } = string.Empty;   // Default to empty string if not set

        [BsonElement("companyName")]
        public string? CompanyName { get; set; }

        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonRequired, BsonElement("role"), BsonRepresentation(BsonType.Int32)]
        public UserRoleEnum Role { get; set; }

        [BsonRequired, BsonElement("status"), BsonRepresentation(BsonType.Int32)]
        public UserStatusEnum Status { get; set; } = UserStatusEnum.PENDING;   // Default to PENDING
    }
}




