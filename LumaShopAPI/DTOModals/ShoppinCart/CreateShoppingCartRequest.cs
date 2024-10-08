// This class is used to create a request for creating a shopping cart

using System.ComponentModel.DataAnnotations;

namespace LumaShopAPI.DTOModals.ShoppinCart
{
    public class CreateShoppingCartRequest
    {
        [Required]
        public string CustomerId { get; set; }

        [Required]
        public List<CreateCartItemRequest> Items { get; set; }

        [Required]
        public int TotalAmount { get; set; }
    }

    public class CreateCartItemRequest
    {
        [Required]
        public string ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int Price { get; set; }
    }
}
