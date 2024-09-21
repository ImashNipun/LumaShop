using System.ComponentModel.DataAnnotations;

namespace LumaShopAPI.DTOModals.Product
{
    public class UpdateProductRequest
    {
        public string Name { get; set; }
      
        public string Description { get; set; }

        public decimal Price { get; set; }

        public string Category { get; set; }

        public bool IsArchived { get; set; } = false;

        public int StockQuantity { get; set; }

        public DimentionDTO Dimensions { get; set; }

        public string Material { get; set; } = string.Empty;

        public List<string> ColorOptions { get; set; }

        public double Weight { get; set; }

        public bool AssemblyRequired { get; set; } = false;

        public List<string> ProductImages { get; set; }

        public int? WarrantyPeriod { get; set; }

        public bool IsFeatured { get; set; } = false;

        public string ListingId { get; set; }
    }
}
