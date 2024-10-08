/*
 * This controller handles operations related to the shopping cart in the LumaShopAPI.
 * It provides endpoints for creating, retrieving, updating, and deleting shopping cart entries.
 * The controller ensures that only authorized customers can access its methods.
 */

using LumaShopAPI.DTOModals.Common;
using LumaShopAPI.DTOModals.ShoppinCart;
using LumaShopAPI.Entities;
using LumaShopAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LumaShopAPI.Controllers
{
    [ApiController]
    [Route("shoppingcart")]
    [Authorize] 
    public class ShoppingCartController : Controller
    {
        private readonly ShoppingCartService _shoppingCartService;

        public ShoppingCartController(ShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpPost]
        [Authorize(Roles = "CUSTOMER")]

        // Creates a new shopping cart for a customer with the specified items and total amount.

        public async Task<IActionResult> CreateShoppingCart([FromBody] CreateShoppingCartRequest request)
        {
            try
            {
                var shoppingCart = new ShoppingCart
                {
                    CustomerId = request.CustomerId,
                    Items = request.Items.Select(item => new CartItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price
                    }).ToList(),
                    TotalAmount = request.TotalAmount
                };

                var createdCart = await _shoppingCartService.CreateShoppingCartAsync(shoppingCart);

                return CreatedAtAction(nameof(GetShoppingCartById), new { id = createdCart.Id }, new APIResponse
                {
                    Status = "success",
                    Message = "Resource created successfully",
                    Data = createdCart,
                    Errors = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse
                {
                    Status = "error",
                    Message = "An unexpected error occurred.",
                    Data = null,
                    Errors = new[] { ex.Message }
                });
            }
        }

        
        [HttpGet("{id}")]
        [Authorize(Roles = "CUSTOMER")]

        // Retrieves a shopping cart by its unique identifier (ID) and returns the cart details.

        public async Task<IActionResult> GetShoppingCartById(string id)
        {
            try
            {
                var cart = await _shoppingCartService.GetShoppingCartByIdAsync(id);
                if (cart == null)
                {
                    return StatusCode(404, new APIResponse
                    {
                        Status = "error",
                        Message = "An unexpected error occurred.",
                        Data = null,
                        Errors = "Shopping cart not found"
                    });
                }
                return StatusCode(200, new APIResponse
                {
                    Status = "success",
                    Message = "Resource fetched successfully",
                    Data = cart,
                    Errors = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse
                {
                    Status = "error",
                    Message = "An unexpected error occurred.",
                    Data = null,
                    Errors = new[] { ex.Message }
                });
            }
        }

      
        [HttpPut("{id}")]
        [Authorize(Roles = "CUSTOMER")]

        // Updates the shopping cart with the specified ID using the provided request data.
        public async Task<IActionResult> UpdateShoppingCart(string id, [FromBody] UpdateShoppingCartRequest request)
        {
            try
            {
                var updatedCart = new ShoppingCart
                {
                    Items = request.Items.Select(item => new CartItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price
                    }).ToList(),
                    TotalAmount = request.TotalAmount,
                    UpdatedAt = DateTime.UtcNow
                };

                var result = await _shoppingCartService.UpdateShoppingCartAsync(id, updatedCart);
                if (result == null)
                {
                    return StatusCode(404, new APIResponse
                    {
                        Status = "error",
                        Message = "An unexpected error occurred.",
                        Data = null,
                        Errors = "Shopping cart not found"
                    });
                }
                return StatusCode(200, new APIResponse
                {
                    Status = "success",
                    Message = "Resource fetched successfully",
                    Data = result,
                    Errors = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse
                {
                    Status = "error",
                    Message = "An unexpected error occurred.",
                    Data = null,
                    Errors = new[] { ex.Message }
                });
            }
        }

       
        [HttpDelete("{id}")]
        [Authorize(Roles = "CUSTOMER")]

        // Deletes the shopping cart identified by its ID and returns the status of the operation.

        public async Task<IActionResult> DeleteShoppingCart(string id)
        {
            try
            {
                var result = await _shoppingCartService.DeleteShoppingCartAsync(id);
                if (!result)
                {
                    return StatusCode(404, new APIResponse
                    {
                        Status = "error",
                        Message = "An unexpected error occurred.",
                        Data = null,
                        Errors = "Shopping cart not found"
                    });
                }
                return StatusCode(200, new APIResponse
                {
                    Status = "success",
                    Message = "Resource fetched successfully",
                    Data = null,
                    Errors = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse
                {
                    Status = "error",
                    Message = "An unexpected error occurred.",
                    Data = null,
                    Errors = new[] { ex.Message }
                });
            }
        }
    }
}
