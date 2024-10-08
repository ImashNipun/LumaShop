// This class is use to create a response for registering a user

namespace LumaShopAPI.DTOModals.User
{
    public class RegisterResponse
    {
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; }
    }
}
