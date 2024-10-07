using LumaShopAPI.DTOModals.Common;
using LumaShopAPI.DTOModals.User;
using LumaShopAPI.Entities;
using LumaShopAPI.LumaShopEnum;
using LumaShopAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LumaShopAPI.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        // Get all users
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return StatusCode(200, new APIResponse
                {
                    Status = "success",
                    Message = "Users retrieved successfully.",
                    Data = users,
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
                }); ;
            }

        }

        // Get a user by Id
        [HttpGet("{id}", Name = "GetUserById")]
        public async Task<ActionResult<User>> GetUserById(string id)
        {
            try
            {

                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return StatusCode(404, new APIResponse
                    {
                        Status = "error",
                        Message = "Resource not found",
                        Data = null,
                        Errors = new[] { "User not found." }
                    });
                }
                return StatusCode(200, new APIResponse
                {
                    Status = "success",
                    Message = "User retrieved successfully.",
                    Data = user,
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

        // Update an existing user
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchUser(string id, [FromBody] UserUpdateRequest userUpdateRequest)
        {
            try
            {
                if (userUpdateRequest == null)
                {
                    return StatusCode(400, new APIResponse
                    {
                        Status = "error",
                        Message = "Invalid update request.",
                        Data = null,
                        Errors = new[] { "Invalid update request." }
                    });
                }

                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return StatusCode(404, new APIResponse
                    {
                        Status = "error",
                        Message = "Resource not found",
                        Data = null,
                        Errors = new[] { "User not found." }
                    });

                }

                // Role-specific logic for handling updates
                if (user.Role == UserRoleEnum.VENDOR)
                {
                    user.CompanyName = userUpdateRequest.CompanyName ?? user.CompanyName;
                    user.Description = userUpdateRequest.Description ?? user.Description;
                }
                else
                {
                    // For non-Vendor roles, only update FirstName and LastName
                    if (!string.IsNullOrWhiteSpace(userUpdateRequest.FirstName))
                    {
                        user.FirstName = userUpdateRequest.FirstName;
                    }

                    if (!string.IsNullOrWhiteSpace(userUpdateRequest.LastName))
                    {
                        user.LastName = userUpdateRequest.LastName;
                    }

                    // Ensure CompanyName and Description are left as null for non-Vendor roles
                    user.CompanyName = null;
                    user.Description = null;
                }

                // Always allow Status update
                user.Status = userUpdateRequest.Status;

                // Save the updated user
                await _userService.UpdateUserAsync(id, user);
                return StatusCode(200, new APIResponse
                {
                    Status = "success",
                    Message = "User updated successfully.",
                    Data = user,
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


        // Delete a user
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return StatusCode(404, new APIResponse
                    {
                        Status = "error",
                        Message = "Resource not found",
                        Data = null,
                        Errors = new[] { "User not found." }
                    });
                }

                await _userService.DeleteUserAsync(id);
                return StatusCode(200, new APIResponse
                {
                    Status = "success",
                    Message = "User deleted successfully.",
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
