using System.ComponentModel.DataAnnotations;

namespace LumaShopAPI.DTOModals.ShoppinCart
{
    public class CreateShoppingCartRequest
    {
        [Required]
        public string CustomerId { get; set; } // ID of the customer

        [Required]
        public List<CreateCartItemRequest> Items { get; set; } // List of items to be added to the cart

        [Required]
        public int TotalAmount { get; set; } // Total cost of items in the cart
    }

    public class CreateCartItemRequest
    {
        [Required]
        public string ProductId { get; set; } // ID of the product

        [Required]
        public int Quantity { get; set; } // Quantity of the product

        [Required]
        public int Price { get; set; } // Price of one unit of the product
    }
}
