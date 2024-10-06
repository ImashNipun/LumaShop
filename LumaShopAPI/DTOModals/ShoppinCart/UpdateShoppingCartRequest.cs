using System.ComponentModel.DataAnnotations;

namespace LumaShopAPI.DTOModals.ShoppinCart
{
    public class UpdateShoppingCartRequest
    {
        [Required]
        public List<UpdateCartItemRequest> Items { get; set; } // Updated list of items in the cart

        [Required]
        public int TotalAmount { get; set; } // Updated total cost of items in the cart
    }

    public class UpdateCartItemRequest
    {
        [Required]
        public string ProductId { get; set; } // ID of the product

        [Required]
        public int Quantity { get; set; } // Updated quantity of the product

        [Required]
        public int Price { get; set; } // Updated price of one unit of the product
    }
}
