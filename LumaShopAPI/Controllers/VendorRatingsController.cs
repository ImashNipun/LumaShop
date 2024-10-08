/*
 * This controller handles operations related to vendor ratings.
 * It includes methods for fetching, creating, updating, and deleting vendor ratings.
 * Access is restricted based on user roles, ensuring only authorized users can perform actions.
 */


using LumaShopAPI.DTOModals.Common;
using LumaShopAPI.DTOModals.VendorRating;
using LumaShopAPI.Entities;
using LumaShopAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LumaShopAPI.Controllers
{
    [ApiController]
    [Route("ratings")]
    [Authorize]
    public class VendorRatingsController : Controller
    {
        private readonly VendorRatingService _vendorRatingService;

        public VendorRatingsController(VendorRatingService vendorRatingService)
        {
            _vendorRatingService = vendorRatingService;
        }

        [HttpGet]
        [Authorize(Roles = "CUSTOMER,CSR,ADMIN,VENDOR")]
        /*
         * This method retrieves a list of all vendor ratings from the database and returns them in the response.
         * It handles exceptions and returns appropriate status codes and messages.
         */
      
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
        [Authorize(Roles = "CUSTOMER,CSR,ADMIN,VENDOR")]

        /*
         * This method retrieves a specific vendor rating by its ID from the database and returns it in the response.
         * It handles exceptions and returns appropriate status codes and messages.
         */

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
        [Authorize(Roles = "CUSTOMER,ADMIN")]

        /*
         * This method creates a new vendor rating in the database and returns the newly created rating in the response.
         * It handles exceptions and returns appropriate status codes and messages.
         */
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
        [Authorize(Roles = "CUSTOMER,ADMIN")]
        
        /*
         * This method updates an existing vendor rating in the database and returns the updated rating in the response.
         * It handles exceptions and returns appropriate status codes and messages.
         */

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
        [Authorize(Roles = "CUSTOMER,ADMIN")]

        /*
         * This method deletes a vendor rating from the database based on its ID and returns the status of the operation.
         * It handles exceptions and returns appropriate status codes and messages.
         */

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
