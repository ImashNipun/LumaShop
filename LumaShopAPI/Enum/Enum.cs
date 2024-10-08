// Defines enums representing user roles and statuses within the LumaShop API.

namespace LumaShopAPI.LumaShopEnum
{
    public enum UserRoleEnum
    {
        CUSTOMER = 1,
        VENDOR = 2,
        ADMIN = 3,
        CSR = 4
    }

    public enum UserStatusEnum
    {
        ACTIVE = 1,
        INACTIVE = 2,
        PENDING = 3
    }

    public enum OrderStatusEnum
    {
        PENDING = 5,
        INPROGRESS = 10,
        DELIVERED = 15,
        CANCELLED = 20,
        COMPLETED = 25
    }

}
