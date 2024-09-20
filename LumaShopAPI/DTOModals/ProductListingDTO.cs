using System.ComponentModel.DataAnnotations;

namespace LumaShopAPI.DTOModals
{
    public class ProductListingDTO
    {
        public string Id { get; set; }  // Optional for creation, required for update

        [Required]
        public string Name { get; set; }  // Required for both create and update

        public string Description { get; set; }

        [Required]
        public string VendorId { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
