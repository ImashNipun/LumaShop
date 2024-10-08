/*
 * This class represents a product listing in the LumaShop API. 
 * It includes properties for storing details about the product Listing
 * such as its ID, name, description, vendor ID, active status, 
 * and timestamps for creation and last update.
 */


using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LumaShopAPI.Entities
{
    public class ProductListing
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [BsonElement("vendorId")]
        public string VendorId { get; set; }

        [Required]
        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

}
