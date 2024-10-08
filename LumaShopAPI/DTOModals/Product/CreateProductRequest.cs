// This is used to create a object for the request of creating product

using System.ComponentModel.DataAnnotations;

namespace LumaShopAPI.DTOModals.Product
{
    public class CreateProductRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string VendorId { get; set; }

        public bool IsArchived { get; set; } = false;

        [Required]
        public int StockQuantity { get; set; }

        public DimentionDTO Dimensions { get; set; }

        public string Material { get; set; } = string.Empty;

        public List<string> ColorOptions { get; set; }

        public double Weight { get; set; }

        public bool AssemblyRequired { get; set; } = false;

        public List<string> ProductImages { get; set; }

        public int? WarrantyPeriod { get; set; }

        public bool IsFeatured { get; set; } = false;

        [Required]
        public string ListingId { get; set; }
    }
}
