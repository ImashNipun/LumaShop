using LumaShopAPI.DTOModals;
using LumaShopAPI.Entities;
using LumaShopAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LumaShopAPI.Controllers
{

    [ApiController]
    [Route("productLiating")]
    public class ProductListingController : Controller
    {
        private readonly ProductListingService _productListingService;

        public ProductListingController(ProductListingService productListingService)
        {
            _productListingService = productListingService;
        }

        // Create a new ProductListing
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductListingDTO productListingDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var productListing = new ProductListing
            {
                Name = productListingDto.Name,
                Description = productListingDto.Description,
                VendorId = productListingDto.VendorId,
                IsActive = productListingDto.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdProductListing = await _productListingService.CreateAsync(productListing);
            return CreatedAtAction(nameof(GetById), new { id = createdProductListing.Id }, createdProductListing);
        }

        // Get all ProductListings
        [HttpGet]
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
        public async Task<IActionResult> Update(string id, [FromBody] ProductListingDTO productListingDto)
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
