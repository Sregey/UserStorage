using System;
using System.Collections.Generic;
using System.Linq;
using UserStorageServices.Validation;

namespace UserStorageServices
{
    public class UserStorageServiceMaster : UserStorageServiceBase
    {
        private event EventHandler<UserStorageModifiedEventArgs> userAdded = delegate { };
        private event EventHandler<UserStorageModifiedEventArgs> userRemoved = delegate { };

        private readonly IIdGenerator idGenerator;
        private readonly IValidator<User> userValidator;

        public UserStorageServiceMaster(IEnumerable<INotificationSubscriber> slaveServices)
        {
            slaveServices = slaveServices ?? Enumerable.Empty<INotificationSubscriber>();
            foreach (var subscriber in slaveServices)
            {
                AddSubscriber(subscriber);
            }


            idGenerator = new IdGenerator();
            userValidator = new UserValidator();
        }

        public override UserStorageServiceMode ServiceMode => UserStorageServiceMode.MasterMode;

        public override void Add(User user)
        {
            userValidator.Validate(user);

            user.Id = idGenerator.Generate();
            users.Add(user);

            userAdded(this, new UserStorageModifiedEventArgs(user));
        }

        public override void RemoveFirst(Predicate<User> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            int i;
            for (i = 0; i < users.Count; i++)
            {
                if (predicate(users[i]))
                {
                    break;
                }
            }

            if (i < users.Count)
            {
                var removedUser = users[i];
                users.RemoveAt(i);
                userRemoved(this, new UserStorageModifiedEventArgs(removedUser));
            }
        }

        public override void RemoveAll(Predicate<User> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            var removedUsers = Search(predicate);
            users.RemoveAll(predicate);

            foreach (var removedUser in removedUsers)
            {
                userRemoved(this, new UserStorageModifiedEventArgs(removedUser));
            }
        }

        public void AddSubscriber(INotificationSubscriber subscriber)
        {
            if (subscriber == null)
            {
                throw new ArgumentNullException(nameof(subscriber));
            }

            userAdded += subscriber.UserAdded;
            userRemoved += subscriber.UserRemoved;
        }

        public void RemoveSubscriber(INotificationSubscriber subscriber)
        {
            if (subscriber == null)
            {
                throw new ArgumentNullException(nameof(subscriber));
            }

            userAdded -= subscriber.UserAdded;
            userRemoved -= subscriber.UserRemoved;
        }
    }
}
