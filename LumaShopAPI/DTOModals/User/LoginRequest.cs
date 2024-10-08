// This class is used to create a request for logging in a user

using System.ComponentModel.DataAnnotations;

namespace LumaShopAPI.DTOModals.User
{
    public class LoginRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
