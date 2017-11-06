namespace UserStorageServices.UserStorage
{
    public class UserStorageServiceSlaveLog : UserStorageServiceLog<UserStorageServiceSlave>, INotificationSubscriber
    {
        public UserStorageServiceSlaveLog(UserStorageServiceSlave userStorageService)
            : base(userStorageService)
        {
        }

        public void UserAdded(object sender, UserStorageModifiedEventArgs args)
        {
            UserStorageService.UserAdded(sender, args);
        }

        public void UserRemoved(object sender, UserStorageModifiedEventArgs args)
        {
            UserStorageService.UserRemoved(sender, args);
        }
    }
}
