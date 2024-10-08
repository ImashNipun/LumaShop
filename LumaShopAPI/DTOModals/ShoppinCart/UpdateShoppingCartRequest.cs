// This class is used to create a request for updating a shopping cart

using System.ComponentModel.DataAnnotations;

namespace LumaShopAPI.DTOModals.ShoppinCart
{
    public class UpdateShoppingCartRequest
    {
        [Required]
        public List<UpdateCartItemRequest> Items { get; set; }

        [Required]
        public int TotalAmount { get; set; }
    }

    public class UpdateCartItemRequest
    {
        [Required]
        public string ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int Price { get; set; }
    }
}
