using LumaShopAPI.DTOModals.Common;
using LumaShopAPI.DTOModals.VendorRating;
using LumaShopAPI.Entities;
using LumaShopAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LumaShopAPI.Controllers
{
    [ApiController]
    [Route("ratings")]
    public class VendorRatingsController : Controller
    {
        private readonly VendorRatingService _vendorRatingService;

        public VendorRatingsController(VendorRatingService vendorRatingService)
        {
            _vendorRatingService = vendorRatingService;
        }

        [HttpGet]
        public async Task<ActionResult<List<VendorRatings>>> GetAll()
        {
            try
            {
                var result = await _vendorRatingService.GetAllAsync();
                return StatusCode(200, new APIResponse
                {
                    Status = "success",
                    Message = "Ratings fetched successfully",
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
        public async Task<ActionResult<VendorRatings>> GetById(string id)
        {
            try
            {
                var result = await _vendorRatingService.GetByIdAsync(id);
                if (result == null) return StatusCode(404, new APIResponse
                {
                    Status = "error",
                    Message = "Rating not found!",
                    Data = null,
                    Errors = null
                });


                return StatusCode(200, new APIResponse
                {
                    Status = "success",
                    Message = "Ratings fetched successfully",
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
        public async Task<ActionResult<VendorRatings>> Create(CreateVendorRatingRequest request)
        {
            try
            {
                var newRating = new VendorRatings
                {
                    VendorId = request.VendorId,
                    CustomerId = request.CustomerId,
                    Rating = request.Rating,
                    Comment = request.Comment
                };
                var createdRating = await _vendorRatingService.CreateAsync(newRating);
                return CreatedAtAction(nameof(GetById), new { id = createdRating.Id }, new APIResponse
                {
                    Status = "success",
                    Message = "Ratings fetched successfully",
                    Data = createdRating,
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

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(string id, UpdateVendorRatingRequest request)
        {
            try
            {
                var existingRating = await _vendorRatingService.GetByIdAsync(id);
                if (existingRating == null) return StatusCode(404, new APIResponse
                {
                    Status = "error",
                    Message = "Rating not found!",
                    Data = null,
                    Errors = null
                });

                if (request.Rating.HasValue)
                {
                    existingRating.Rating = request.Rating.Value;
                }

                if (!string.IsNullOrEmpty(request.Comment))
                {
                    existingRating.Comment = request.Comment;
                }

                existingRating.UpdatedAt = DateTime.UtcNow;

                await _vendorRatingService.UpdateAsync(id, existingRating);
                return StatusCode(200, new APIResponse
                {
                    Status = "success",
                    Message = "Ratings updated successfully",
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
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var existingRating = await _vendorRatingService.GetByIdAsync(id);
                if (existingRating == null) return StatusCode(404, new APIResponse
                {
                    Status = "error",
                    Message = "Rating not found!",
                    Data = null,
                    Errors = null
                });

                await _vendorRatingService.DeleteAsync(id);
                return StatusCode(200, new APIResponse
                {
                    Status = "success",
                    Message = "Ratings deleted successfully",
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
