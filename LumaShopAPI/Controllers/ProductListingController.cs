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

        // Create a new ProductListing
        [HttpPost]
        [Authorize(Roles = "ADMIN,VENDOR")]
        public async Task<IActionResult> Create([FromBody] CreateProductListingRquest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var vendorUser = await _userService.GetUserByIdAsync(request.VendorId);
                if (vendorUser == null || vendorUser.Role != UserRoleEnum.VENDOR)
                {
                    return BadRequest(new APIResponse
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
                    IsActive = true,
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

        // Get all ProductListings
        [HttpGet]
        [Authorize(Roles = "ADMIN,VENDOR")]
        public async Task<ActionResult<List<ProductListing>>> GetAll()
        {
            var productListings = await _productListingService.GetAllAsync();
            return Ok(productListings);
        }

        // Get a ProductListing by Id
        [HttpGet("{id:length(24)}", Name = "GetById")]
        public async Task<ActionResult<ProductListing>> GetById(string id)
        {
            var productListing = await _productListingService.GetByIdAsync(id);
            if (productListing == null)
                return NotFound();

            return Ok(productListing);
        }

        // Update a ProductListing by Id
        [HttpPatch("{id:length(24)}")]
        [Authorize(Roles = "ADMIN,VENDOR")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateProductListingRquest productListingDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingProductListing = await _productListingService.GetByIdAsync(id);
            if (existingProductListing == null)
                return NotFound();

            // Update only the fields that are provided in the DTO
            if (productListingDto.Name != null)
                existingProductListing.Name = productListingDto.Name;

            if (productListingDto.Description != null)
                existingProductListing.Description = productListingDto.Description;

            if (productListingDto.VendorId != null)
                existingProductListing.VendorId = productListingDto.VendorId;

            existingProductListing.IsActive = productListingDto.IsActive;

            existingProductListing.UpdatedAt = DateTime.UtcNow;

            await _productListingService.UpdateAsync(id, existingProductListing);
            return NoContent();
        }

        // Delete a ProductListing by Id
        [HttpDelete("{id:length(24)}")]
        [Authorize(Roles = "ADMIN,VENDOR")]
        public async Task<IActionResult> Delete(string id)
        {
            var productListing = await _productListingService.GetByIdAsync(id);
            if (productListing == null)
                return NotFound();

            await _productListingService.DeleteAsync(id);
            return NoContent();
        }
    }
}
