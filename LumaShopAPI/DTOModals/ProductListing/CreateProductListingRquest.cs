// This class is use to create request for creating product listing

using System.ComponentModel.DataAnnotations;

namespace LumaShopAPI.DTOModals.ProductListing
{
    public class CreateProductListingRquest
    {

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public string VendorId { get; set; }
    }
}
