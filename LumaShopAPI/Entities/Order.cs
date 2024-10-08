/*
 * Contains the definition of the Order and Item classes used for handling orders in the LumaShop API.
 */


using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using LumaShopAPI.LumaShopEnum;

namespace LumaShopAPI.Entities
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("customerId")]
        [BsonRequired]
        public string CustomerId { get; set; }

        [BsonElement("items")]
        [BsonRequired]
        public List<Item> Items { get; set; }

        [BsonElement("totalAmount")]
        [BsonRequired]
        public decimal TotalAmount { get; set; }

        [BsonElement("status")]
        [BsonRequired]
        public OrderStatusEnum Status { get; set; } = OrderStatusEnum.PENDING;

        [BsonElement("createdBy")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CreatedBy { get; set; }

        [BsonElement("createdAt")]
        [BsonDateTimeOptions]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        [BsonDateTimeOptions]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isArchive")]
        [BsonDefaultValue(false)]
        public bool IsArchive { get; set; } = false;

    }

    public class Item
    {
        [BsonRequired]
        [BsonElement("productId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; }

        [BsonRequired]
        [BsonElement("quantity")]
        public int Quantity { get; set; }

        [BsonRequired]
        [BsonElement("price")]
        public decimal Price { get; set; }
    }
}
