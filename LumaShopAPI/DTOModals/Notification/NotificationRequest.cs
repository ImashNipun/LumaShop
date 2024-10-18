namespace LumaShopAPI.DTOModals.Notification
{
    public class NotificationRequest
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string DeviceToken { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
