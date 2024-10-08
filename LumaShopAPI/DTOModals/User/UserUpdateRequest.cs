// This class is used to create a request for updating a user

using LumaShopAPI.LumaShopEnum;
using System.ComponentModel.DataAnnotations;

namespace LumaShopAPI.DTOModals.User
{
    public class UserUpdateRequest
    {
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? CompanyName { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public UserStatusEnum Status { get; set; } = UserStatusEnum.PENDING;
    }
}
