// This class is used to create a request for registering a user

using LumaShopAPI.LumaShopEnum;
using System.ComponentModel.DataAnnotations;

namespace LumaShopAPI.DTOModals.User
{
    public class RegisterRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;

        [Required]
        public UserRoleEnum Role { get; set; } = UserRoleEnum.CUSTOMER;

        [Required]
        public UserStatusEnum Status { get; set; } = UserStatusEnum.PENDING;

        public string? CompanyName { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Required, DataType(DataType.Password), Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
