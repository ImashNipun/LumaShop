// This class is use to create request for updating product listing

using System.ComponentModel.DataAnnotations;

namespace LumaShopAPI.DTOModals.ProductListing
{
    public class UpdateProductListingRquest
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public string VendorId { get; set; }
        public bool IsActive { get; set; }
    }
}
