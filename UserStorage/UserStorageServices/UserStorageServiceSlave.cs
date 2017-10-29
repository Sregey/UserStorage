using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorageServices
{
    public class UserStorageServiceSlave : UserStorageServiceBase, INotificationSubscriber
    {
        private const string NOT_SUPPORTED_MESSAGE = "This service is slave";

        public override UserStorageServiceMode ServiceMode => UserStorageServiceMode.SlaveMode;

        public override void Add(User user)
        {
            throw new NotSupportedException(NOT_SUPPORTED_MESSAGE);
        }

        public override void RemoveFirst(Predicate<User> predicate)
        {
            throw new NotSupportedException(NOT_SUPPORTED_MESSAGE);
        }

        public override void RemoveAll(Predicate<User> predicate)
        {
            throw new NotSupportedException(NOT_SUPPORTED_MESSAGE);
        }

        public void UserAdded(object sender, UserStorageModifiedEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            users.Add(args.User);
        }

        public void UserRemoved(object sender, UserStorageModifiedEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            users.Remove(args.User);
        }
    }
}
