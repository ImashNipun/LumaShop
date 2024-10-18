/*
 * This controller manages the operations related to product listings, 
 * including creating, retrieving, updating, and deleting product listings.
 * It is secured for authorized users with ADMIN and VENDOR roles.
 */

using Amazon.Runtime.Internal;
using LumaShopAPI.DTOModals;
using LumaShopAPI.DTOModals.Common;
using LumaShopAPI.DTOModals.ProductListing;
using LumaShopAPI.Entities;
using LumaShopAPI.LumaShopEnum;
using LumaShopAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LumaShopAPI.Controllers
{

    [ApiController]
    [Route("productListing")]
    [Authorize]
    public class ProductListingController : Controller
    {
        private readonly ProductListingService _productListingService;
        private readonly UserService _userService;

        public ProductListingController(ProductListingService productListingService, UserService userService)
        {
            _productListingService = productListingService;
            _userService = userService;
        }

        
        [HttpPost]
        [Authorize(Roles = "ADMIN,VENDOR")]

        /*Create a new ProductListing. This method validates the vendor ID and 
         * creates a product listing if the vendor exists and has the correct role.
         */

        public async Task<IActionResult> Create([FromBody] CreateProductListingRquest request)
        {
            try
            {

                var vendorUser = await _userService.GetUserByIdAsync(request.VendorId);
                if (vendorUser == null || vendorUser.Role != UserRoleEnum.VENDOR)
                {
                    return StatusCode(400, new APIResponse
                    {
                        Status = "error",
                        Message = "Vendor not found or does not have the required role.",
                        Data = null,
                        Errors = new[] { "Invalid vendor ID." }
                    });
                }

                var productListing = new ProductListing
                {
                    Name = request.Name,
                    Description = request.Description,
                    VendorId = request.VendorId,
                    IsActive = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var createdProductListing = await _productListingService.CreateAsync(productListing);
                return CreatedAtAction(nameof(GetById), new { id = createdProductListing.Id }, new APIResponse
                {
                    Status = "success",
                    Message = "Resource created successfully",
                    Data = createdProductListing,
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

        [HttpGet]
        [Authorize(Roles = "ADMIN,VENDOR")]

        /*
         * Retrieve all ProductListings. This method fetches all product listings 
         * and returns them in the response.
         */
            
        public async Task<ActionResult<List<ProductListing>>> GetAll()
        {
            try
            {
                var productListings = await _productListingService.GetAllAsync();
                return StatusCode(200, new APIResponse
                {
                    Status = "success",
                    Message = "Resource fetched successfully",
                    Data = productListings,
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

        [HttpGet("{id:length(24)}", Name = "GetById")]

        /*
         * Retrieve a ProductListing by Id. This method fetches a product listing 
         * by its ID and returns it in the response.
         */
        public async Task<ActionResult<ProductListing>> GetById(string id)
        {
            try
            {
                var productListing = await _productListingService.GetByIdAsync(id);
                if (productListing == null)
                    return StatusCode(404, new APIResponse
                    {
                        Status = "error",
                        Message = "Resource not found",
                        Data = null,
                        Errors = new[] { "Product listing not found." }
                    });
                    
                return StatusCode(200, new APIResponse
                {
                    Status = "success",
                    Message = "Resource fetched successfully",
                    Data = productListing,
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

        [HttpPatch("{id:length(24)}")]
        [Authorize(Roles = "ADMIN,VENDOR")]

        /*
         * Update a ProductListing by Id. This method updates a product listing 
         * based on the provided ID and request body.
         */
        public async Task<IActionResult> Update(string id, [FromBody] UpdateProductListingRquest productListingDto)
        {
            try
            {
                var existingProductListing = await _productListingService.GetByIdAsync(id);
                if (existingProductListing == null)
                    return StatusCode(404, new APIResponse
                    {
                        Status = "error",
                        Message = "Resource not found",
                        Data = null,
                        Errors = new[] { "Product listing not found." }
                    });

                if (productListingDto.Name != null)
                    existingProductListing.Name = productListingDto.Name;

                if (productListingDto.Description != null)
                    existingProductListing.Description = productListingDto.Description;

                if (productListingDto.VendorId != null)
                    existingProductListing.VendorId = productListingDto.VendorId;

                existingProductListing.IsActive = productListingDto.IsActive;

                existingProductListing.UpdatedAt = DateTime.UtcNow;

                await _productListingService.UpdateAsync(id, existingProductListing);
                return StatusCode(200, new APIResponse
                {
                    Status = "success",
                    Message = "Resource updated successfully",
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

        [HttpDelete("{id:length(24)}")]
        [Authorize(Roles = "ADMIN,VENDOR")]

        /*
         * Delete a ProductListing by Id. This method deletes a product listing 
         * based on the provided ID.
         */
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var productListing = await _productListingService.GetByIdAsync(id);
                if (productListing == null)
                    return StatusCode(404, new APIResponse
                    {
                        Status = "error",
                        Message = "Resource not found",
                        Data = null,
                        Errors = new[] { "Product listing not found." }
                    });

                await _productListingService.DeleteAsync(id);
                return StatusCode(200, new APIResponse
                {
                    Status = "success",
                    Message = "Resource deleted successfully",
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

        [HttpGet("GetByVendorId/{vendorId}")]
        [Authorize(Roles = "ADMIN,VENDOR")]

        /*
         * Retrieve a ProductListing by Vendor Id. This method fetches a product listing 
         * by its Vendor ID and returns it in the response.
         */
        public async Task<ActionResult<ProductListing>> GetListingsByVendorId(string vendorId)
        {
            try
            {
                var productListing = await _productListingService.GetAllListingByVendorIdAsync(vendorId);
                if (productListing == null)
                    return StatusCode(404, new APIResponse
                    {
                        Status = "error",
                        Message = "Resource not found",
                        Data = null,
                        Errors = new[] { "Product listing not found." }
                    });

                return StatusCode(200, new APIResponse
                {
                    Status = "success",
                    Message = "Resource fetched successfully",
                    Data = productListing,
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
