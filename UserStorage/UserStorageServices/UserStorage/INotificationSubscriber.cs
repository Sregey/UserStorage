namespace UserStorageServices.UserStorage
{
    public interface INotificationSubscriber
    {
        void UserAdded(object sender, UserStorageModifiedEventArgs args);

        void UserRemoved(object sender, UserStorageModifiedEventArgs args);
    }
}
