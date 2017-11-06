using System;
using UserStorageServices.Repositories;

namespace UserStorageServices
{
    public class UserStorageServiceSlave : UserStorageServiceBase, INotificationSubscriber
    {
        private const string NotSupportedMessage = "This service is slave";

        public UserStorageServiceSlave(IUserRepository userRepository)
            : base(userRepository)
        {
        }

        public override UserStorageServiceMode ServiceMode => UserStorageServiceMode.SlaveMode;

        public override void Add(User user)
        {
            throw new NotSupportedException(NotSupportedMessage);
        }

        public override void Remove(Predicate<User> predicate)
        {
            throw new NotSupportedException(NotSupportedMessage);
        }

        public void UserAdded(object sender, UserStorageModifiedEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            UserRepository.Set(args.User);
        }

        public void UserRemoved(object sender, UserStorageModifiedEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            UserRepository.Delete((u) => u.Id == args.User.Id);
        }
    }
}
