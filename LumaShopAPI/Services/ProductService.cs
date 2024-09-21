using LumaShopAPI.Entities;
using LumaShopAPI.Services.Database;
using MongoDB.Driver;

namespace LumaShopAPI.Services
{
    public class ProductService
    {
        private readonly IMongoCollection<ProductListing> _productListings;

        public ProductService(MongodbService mongodbService)
        {
            _productListings = mongodbService.Database.GetCollection<ProductListing>("products");
        }
    }
}
