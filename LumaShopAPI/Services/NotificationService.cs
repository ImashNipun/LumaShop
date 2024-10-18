using Amazon.Runtime.Internal;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using LumaShopAPI.DTOModals.Notification;

namespace LumaShopAPI.Services
{
    public class NotificationService
    {
        private readonly FirebaseMessaging _messaging;

        public NotificationService()
        {
            _messaging = FirebaseMessaging.DefaultInstance;
        }

        public async Task<string> SendNotificationAsync(NotificationRequest request)
        {
            var message = new Message()
            {
                Notification = new Notification
                {
                    Title = request.Title,
                    Body = request.Body,
                },
                Data = new Dictionary<string, string>()
                {
                    ["FirstName"] = request.FirstName,
                    ["LastName"] = request.LastName
                },
                Token = request.DeviceToken
            };

            var result = await _messaging.SendAsync(message);
            return result;
        }
    }
}
