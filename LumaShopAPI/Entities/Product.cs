/*
 * Containing various properties
 * that describe the product, such as its name, description, price, 
 * category, and other relevant details.
 */

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace LumaShopAPI.Entities
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        [BsonElement("name")]
        public string Name { get; set; }

        [Required]
        [BsonElement("description")]
        public string Description { get; set; }

        [Required]
        [BsonElement("price")]
        public int Price { get; set; }

        [Required]
        [BsonElement("category")]
        public string Category { get; set; }

        [Required]
        [BsonElement("vendorId")]
        public string VendorId { get; set; }

        [BsonElement("isArchived")]
        public bool IsArchived { get; set; } = false;

        [Required]
        [BsonElement("stockQuantity")]
        public int StockQuantity { get; set; }

        [BsonElement("dimensions")]
        public Dimensions? Dimensions { get; set; }

        [BsonElement("material")]
        public string? Material { get; set; } = string.Empty;

        [BsonElement("colorOptions")]
        public List<string>? ColorOptions { get; set; }

        [BsonElement("weight")]
        public double? Weight { get; set; }

        [BsonElement("assemblyRequired")]
        public bool? AssemblyRequired { get; set; } = false;

        [BsonElement("productImages")]
        public List<string>? ProductImages { get; set; }

        [BsonElement("warrantyPeriod")]
        public int? WarrantyPeriod { get; set; }

        [BsonElement("isFeatured")]
        public bool? IsFeatured { get; set; } = false;

        [Required]
        [BsonElement("listingId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ListingId { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class Dimensions
    {
     
        [BsonElement("width")]
        public double? Width { get; set; }

        [BsonElement("height")]
        public double? Height { get; set; }

        [BsonElement("depth")]
        public double? Depth { get; set; }
    }
}
