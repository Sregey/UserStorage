using System;

namespace UserStorageServices.Notification
{
    public interface INotificationReceiver
    {
        event Action<NotificationContainer> Received;

        void Receive(string serializedNotification);
    }
}
