namespace UserStorageServices.Notification
{
    internal interface INotificationSender
    {
        void Send(NotificationContainer notificationContainer);
    }
}
