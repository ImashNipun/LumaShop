/*
 *This class handles HTTP requests related to user management, including operations to get, update, and delete users.
 *It provides endpoints for retrieving all users, a user by ID, updating user information, and deleting a user.
 */

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

       
        [HttpGet]

        /*
         * This method retrieves a list of all users from the database and returns them in the response.
         * It handles exceptions and returns appropriate status codes and messages.
         */

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

        
        [HttpGet("{id}", Name = "GetUserById")]

        /*
         * This method retrieves a user by their ID from the database and returns it in the response.
         * It handles exceptions and returns appropriate status codes and messages.
         */
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

        
        [HttpPatch("{id}")]

        /*
         * This method updates a user's information based on the provided user ID and update request.
         * It handles exceptions and returns appropriate status codes and messages.
         */
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

                
                if (user.Role == UserRoleEnum.VENDOR)
                {
                    user.CompanyName = userUpdateRequest.CompanyName ?? user.CompanyName;
                    user.Description = userUpdateRequest.Description ?? user.Description;
                }
                else
                {
                    
                    if (!string.IsNullOrWhiteSpace(userUpdateRequest.FirstName))
                    {
                        user.FirstName = userUpdateRequest.FirstName;
                    }

                    if (!string.IsNullOrWhiteSpace(userUpdateRequest.LastName))
                    {
                        user.LastName = userUpdateRequest.LastName;
                    }

                    user.CompanyName = null;
                    user.Description = null;
                }

                
                user.Status = userUpdateRequest.Status;

               
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


        
        [HttpDelete("{id}")]

        /*
         * This method deletes a user based on the provided user ID.
         * It handles exceptions and returns appropriate status codes and messages.
         */
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
