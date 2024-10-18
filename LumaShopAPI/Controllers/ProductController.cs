/*
 * This controller manages product-related operations in the LumaShop API. It allows 
 * authorized users to perform CRUD (Create, Read, Update, Delete) operations on 
 * products. The controller interacts with the ProductService to handle business logic 
 * and communicates with the MongoDB database to store and retrieve product data.
 */


using LumaShopAPI.DTOModals.Common;
using LumaShopAPI.DTOModals.Product;
using LumaShopAPI.Entities;
using LumaShopAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace LumaShopAPI.Controllers
{
    [ApiController]
    [Route("product")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Authorize(Roles = "CUSTOMER,CSR,ADMIN,VENDOR")]

        //Asynchronously retrieves all products from the database and returns them in a response.
        public async Task<ActionResult<List<Product>>> GetAll()
        {
            try
            {
                var result = await _productService.GetAllAsync();
                return StatusCode(200, new APIResponse
                {
                    Status = "success",
                    Message = "Producst fetched successfully",
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

        [HttpGet("{id}")]
        [Authorize(Roles = "CUSTOMER,CSR,ADMIN,VENDOR")]

        // Asynchronously retrieves a single product by its ID. Returns 404 if the product is not found.

        public async Task<ActionResult<Product>> GetById(string id)
        {
            try
            {
                var result = await _productService.GetByIdAsync(id);

                if (result == null)
                {
                    return StatusCode(404, new APIResponse
                    {
                        Status = "error",
                        Message = "Product not found!",
                        Data = null,
                        Errors = new[] { "Product not found!" }
                    });
                }

                return StatusCode(200, new APIResponse
                {
                    Status = "success",
                    Message = "Product fetched successfully",
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

        [HttpPost]
        [Authorize(Roles = "ADMIN,VENDOR")]

        // Asynchronously creates a new product based on the provided request data.
        public async Task<ActionResult> Create([FromBody] CreateProductRequest request)
        {
            try
            {
                var product = new Product
                {
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    Category = request.Category,
                    VendorId = request.VendorId,
                    StockQuantity = request.StockQuantity,
                    Dimensions = request.Dimensions != null ? new Dimensions
                    {
                        Width = request.Dimensions.Width,
                        Height = request.Dimensions.Height,
                        Depth = request.Dimensions.Depth
                    } : null,
                    Material = request.Material,
                    ColorOptions = request.ColorOptions,
                    Weight = request.Weight,
                    AssemblyRequired = request.AssemblyRequired,
                    ProductImages = request.ProductImages,
                    WarrantyPeriod = request.WarrantyPeriod,
                    IsFeatured = request.IsFeatured,
                    ListingId = request.ListingId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                var result = await _productService.CreateAsync(product);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, new APIResponse
                {
                    Status = "success",
                    Message = "Product created successfully",
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

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN,VENDOR")]

        // Asynchronously updates an existing product based on the provided request data.
        public async Task<ActionResult> Update(string id, [FromBody] UpdateProductRequest request)
        {
            try
            {
                var existingProduct = await _productService.GetByIdAsync(id);
                if (existingProduct == null)
                {
                    return StatusCode(404, new APIResponse
                    {
                        Status = "error",
                        Message = "Product not found!",
                        Data = null,
                        Errors = new[] { "Product not found!" }
                    });
                }

                if (request.Name != null)
                    existingProduct.Name = request.Name;

                if (request.Description != null)
                    existingProduct.Description = request.Description;

                if (request.Category != null)
                    existingProduct.Category = request.Category;

                if (request.Dimensions != null)
                {
                    existingProduct.Dimensions = new Dimensions
                    {
                        Width = request.Dimensions.Width,
                        Height = request.Dimensions.Height,
                        Depth = request.Dimensions.Depth
                    };
                }

                if (request.Material != null)
                    existingProduct.Material = request.Material;

                if (request.ColorOptions != null)
                    existingProduct.ColorOptions = request.ColorOptions;

                if (request.ProductImages != null)
                    existingProduct.ProductImages = request.ProductImages;

                if (request.WarrantyPeriod.HasValue)
                    existingProduct.WarrantyPeriod = request.WarrantyPeriod.Value;

                if (request.ListingId != null)
                    existingProduct.ListingId = request.ListingId;

                existingProduct.Price = request.Price;
                existingProduct.IsArchived = request.IsArchived;
                existingProduct.StockQuantity = request.StockQuantity;
                existingProduct.Weight = request.Weight;
                existingProduct.AssemblyRequired = request.AssemblyRequired;
                existingProduct.IsFeatured = request.IsFeatured;

                await _productService.UpdateAsync(id, existingProduct);
                return StatusCode(200, new APIResponse
                {
                    Status = "success",
                    Message = "Product updated successfully",
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

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN,VENDOR")]

        // Asynchronously deletes a product based on the provided ID.
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                if (product == null)
                {
                    return StatusCode(404, new APIResponse
                    {
                        Status = "error",
                        Message = "Product not found!",
                        Data = null,
                        Errors = new[] { "Product not found!" }
                    });
                }

                await _productService.DeleteAsync(id);
                return StatusCode(200, new APIResponse
                {
                    Status = "success",
                    Message = "Product deleted successfully",
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


        [HttpGet("ByVendorId/{vendorId}")]
        [Authorize(Roles = "ADMIN,VENDOR")]

        //Asynchronously retrieves all vendor specific products from the database and returns them in a response.
        public async Task<ActionResult<List<Product>>> GetAllProductsByVendorId(string vendorId)
        {
            try
            {
                var result = await _productService.GetAllProductsByVendorIdAsync(vendorId);
                return StatusCode(200, new APIResponse
                {
                    Status = "success",
                    Message = "Producst fetched successfully",
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

        [HttpGet("GetAllProducts")]
        [Authorize(Roles = "ADMIN,VENDOR")]

        //Asynchronously retrieves all products from the database and returns them in a response.
        public async Task<ActionResult<List<Product>>> GetAllProducts()
        {
            try
            {
                var result = await _productService.GetAllProductAsync();
                return StatusCode(200, new APIResponse
                {
                    Status = "success",
                    Message = "Producst fetched successfully",
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
    }
}
