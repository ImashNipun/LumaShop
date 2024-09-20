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
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // Get a user by Id
        [HttpGet("{id:length(24)}", Name = "GetUserById")]
        public async Task<ActionResult<User>> GetUserById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // Update an existing user
        [HttpPatch("{id:length(24)}")]
        public async Task<IActionResult> PatchUser(string id, [FromBody] UserUpdateRequest userUpdateRequest)
        {
            if (userUpdateRequest == null)
            {
                return BadRequest("Invalid update request.");
            }

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
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
            return NoContent();
        }


        // Delete a user
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.DeleteUserAsync(id);
            return NoContent();  // Return NoContent status after successful delete
        }
    }
}
