// This class is use to create a request for creating a vendor rating

using System.ComponentModel.DataAnnotations;

namespace LumaShopAPI.DTOModals.VendorRating
{
    public class CreateVendorRatingRequest
    {
        [Required]
        public string VendorId { get; set; }

        [Required]
        public string CustomerId { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int? Rating { get; set; }

        [StringLength(500, ErrorMessage = "Comment cannot exceed 500 characters.")]
        public string Comment { get; set; } = string.Empty;
    }
}
