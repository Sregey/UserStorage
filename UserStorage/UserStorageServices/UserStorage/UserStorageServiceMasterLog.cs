namespace UserStorageServices.UserStorage
{
    public class UserStorageServiceMasterLog : UserStorageServiceLog<UserStorageServiceMaster>
    {
        public UserStorageServiceMasterLog(UserStorageServiceMaster userStorageService)
            : base(userStorageService)
        {
        }

        public void AddSubscriber(INotificationSubscriber subscriber)
        {
            UserStorageService.AddSubscriber(subscriber);
        }

        public void RemoveSubscriber(INotificationSubscriber subscriber)
        {
            UserStorageService.RemoveSubscriber(subscriber);
        }
    }
}
