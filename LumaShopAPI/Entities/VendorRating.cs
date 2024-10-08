/*
 * This class defines the structure for vendor ratings in the LumaShop API.
 * It represents customer ratings for vendors, including the vendor ID, customer ID, rating,
 * and any comments provided by the customer. The class uses MongoDB attributes to specify
 * the mapping of its properties to the corresponding fields in the MongoDB database.
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
namespace LumaShopAPI.Entities
{

    public class VendorRatings
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("vendorId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string VendorId { get; set; }

        [BsonElement("customerId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CustomerId { get; set; }

        [BsonElement("rating")]
        public int? Rating { get; set; }

        [BsonElement("comment")]
        public string Comment { get; set; } = string.Empty;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

}
