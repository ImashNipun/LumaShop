using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace LumaShopAPI.Entities
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }  // Unique identifier for the product

        [Required]
        [BsonElement("name")]
        public string Name { get; set; }  // Required - Name of the furniture item (string)

        [Required]
        [BsonElement("description")]
        public string Description { get; set; }  // Required - Detailed description of the item (string)

        [Required]
        [BsonElement("price")]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }  // Required - Price of the furniture (decimal)

        [Required]
        [BsonElement("category")]
        public string Category { get; set; }  // Required - Furniture category (string)

        [Required]
        [BsonElement("vendorId")]
        public string VendorId { get; set; }  // Required - Reference to the vendor (ObjectId)

        [BsonElement("isArchived")]
        public bool IsArchived { get; set; } = false;  // Optional - Default is false (boolean)

        [Required]
        [BsonElement("stockQuantity")]
        public int StockQuantity { get; set; }  // Required - Quantity in stock (integer)

        [BsonElement("dimensions")]
        public Dimensions? Dimensions { get; set; }  // Optional - Dimensions of the furniture item (object)

        [BsonElement("material")]
        public string? Material { get; set; } = string.Empty;  // Optional - Material of the furniture (string)

        [BsonElement("colorOptions")]
        public List<string>? ColorOptions { get; set; }  // Optional - Available color options (list of strings)

        [BsonElement("weight")]
        public double? Weight { get; set; }  // Optional - Weight of the furniture item (double)

        [BsonElement("assemblyRequired")]
        public bool? AssemblyRequired { get; set; } = false;  // Optional - Default is false (boolean)

        [BsonElement("productImages")]
        public List<string>? ProductImages { get; set; }  // Optional - URLs of product images (list of strings)

        [BsonElement("warrantyPeriod")]
        public int? WarrantyPeriod { get; set; }  // Optional - Warranty period in months (nullable integer)

        [BsonElement("isFeatured")]
        public bool? IsFeatured { get; set; } = false;  // Optional - Default is false (boolean)

        [Required]
        [BsonElement("listingId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ListingId { get; set; }  // Required - Reference to the product listing it belongs to (ObjectId)

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  // Auto-assigned - Timestamp (DateTime)

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;  // Auto-assigned - Timestamp (DateTime)
    }

    public class Dimensions
    {
     
        [BsonElement("width")]
        public double? Width { get; set; }  // Required - Width (double)

        [BsonElement("height")]
        public double? Height { get; set; }  // Required - Height (double)

        [BsonElement("depth")]
        public double? Depth { get; set; }  // Required - Depth (double)
    }
}
