using LumaShopAPI.Entities;

namespace LumaShopAPI.DTOModals
{
    public class ProductDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public string Category { get; set; }
        public string VendorId { get; set; }
        public bool? IsArchived { get; set; }
        public int? StockQuantity { get; set; }
        public Dimensions Dimensions { get; set; }
        public string Material { get; set; }
        public List<string> ColorOptions { get; set; }
        public double? Weight { get; set; }
        public bool? AssemblyRequired { get; set; }
        public List<string> ProductImages { get; set; }
        public int? WarrantyPeriod { get; set; }
        public bool? IsFeatured { get; set; }
        public string ListingId { get; set; }
    }
}
