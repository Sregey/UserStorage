namespace UserStorageServices.Notification
{
    interface INotificationSerializer
    {
        string Serialize(NotificationContainer container);

        NotificationContainer Deserialize(string container);
    }
}
