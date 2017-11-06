namespace UserStorageServices.Notification
{
    internal interface INotificationSerializer
    {
        string Serialize(NotificationContainer container);

        NotificationContainer Deserialize(string container);
    }
}
