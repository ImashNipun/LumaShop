namespace LumaShopAPI.DTOModals.Common
{
    public class APIResponse
    {
        public string Status { get; set; }
        public object? Data { get; set; }
        public string Message { get; set; }
        public object? Errors { get; set; }
    }
}
