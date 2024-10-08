/*
 * This file contains the definition of the ShoppingCart and CartItem 
 * classes for the LumaShopAPI. The ShoppingCart class represents a user's shopping 
 * cart and contains a list of items, customer ID, total amount, and timestamps 
 * for creation and updates. The CartItem class represents individual items in the 
 * shopping cart, including the product ID, quantity, and price.
 */

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
        public string Id { get; set; } 

        [BsonElement("customerId")]
        [BsonRequired]
        public string CustomerId { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        [Required]
        [BsonElement("totalAmount")]
        public int TotalAmount { get; set; }

        [BsonElement("createdAt")]
        [BsonDateTimeOptions]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        [BsonDateTimeOptions]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class CartItem
    {
        [BsonRequired]
        [BsonElement("productId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; }

        [Required]
        [BsonElement("quantity")]
        public int Quantity { get; set; }

        [Required]
        [BsonElement("price")]
        public int Price { get; set; }
    }
}
