using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace LumaShopAPI.Entities
{
    public class ShoppingCart
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("id")]
        public string Id { get; set; } // Unique ID for the cart

        [BsonElement("customerId")]
        [BsonRequired]
        public string CustomerId { get; set; } // ID of the customer associated with the cart
        public List<CartItem> Items { get; set; } = new List<CartItem>(); // List of items in the cart

        [Required]
        [BsonElement("totalAmount")]
        public int TotalAmount { get; set; } // Total cost of items in the cart

        [BsonElement("createdAt")]
        [BsonDateTimeOptions]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // When the cart was created

        [BsonElement("updatedAt")]
        [BsonDateTimeOptions]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; // When the cart was last updated
    }

    public class CartItem
    {
        [BsonRequired]
        [BsonElement("productId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; } // ID of the product

        [Required]
        [BsonElement("quantity")]
        public int Quantity { get; set; } // Quantity of the product

        [Required]
        [BsonElement("price")]
        public int Price { get; set; } // Price of one unit of the product
    }
}
