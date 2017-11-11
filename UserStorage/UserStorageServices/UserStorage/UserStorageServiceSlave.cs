using System;
using UserStorageServices.Notification;
using UserStorageServices.Repositories;

namespace UserStorageServices.UserStorage
{
    public class UserStorageServiceSlave : UserStorageServiceBase
    {
        private const string NotSupportedMessage = "This service is slave";

        public UserStorageServiceSlave(IUserRepository userRepository)
            : base(userRepository)
        {
            Receiver = new NotificationReceiver();
            Receiver.Received += NotificationReceived;
        }

        public override UserStorageServiceMode ServiceMode => UserStorageServiceMode.SlaveMode;

        public INotificationReceiver Receiver { get; }

        public override void Add(User user)
        {
            throw new NotSupportedException(NotSupportedMessage);
        }

        public override void Remove(Predicate<User> predicate)
        {
            throw new NotSupportedException(NotSupportedMessage);
        }

        private void NotificationReceived(NotificationContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            if (container.Notifications == null)
            {
                throw new ArgumentNullException(nameof(container.Notifications));
            }

            foreach (var notification in container.Notifications)
            {
                if (notification.Type == NotificationType.AddUser)
                {
                    var action = (AddUserActionNotification)notification.Action;
                    UserRepository.Set(action.User);
                }
                else if (notification.Type == NotificationType.DeleteUser)
                {
                    var action = (DeleteUserActionNotification)notification.Action;
                    UserRepository.Delete((u) => u.Id == action.UserId);
                }
            }
        }
    }
}
