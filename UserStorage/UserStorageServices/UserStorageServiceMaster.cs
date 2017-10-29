using System;
using System.Collections.Generic;
using System.Linq;
using UserStorageServices.Validation;

namespace UserStorageServices
{
    public class UserStorageServiceMaster : UserStorageServiceBase
    {
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

        private event EventHandler<UserStorageModifiedEventArgs> UserAdded = delegate { };
        private event EventHandler<UserStorageModifiedEventArgs> UserRemoved = delegate { };

        public override UserStorageServiceMode ServiceMode => UserStorageServiceMode.MasterMode;

        public override void Add(User user)
        {
            userValidator.Validate(user);

            user.Id = idGenerator.Generate();
            users.Add(user);

            OnUserAdded(new UserStorageModifiedEventArgs(user));
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
                OnUserRemoved(new UserStorageModifiedEventArgs(removedUser));
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
                OnUserRemoved(new UserStorageModifiedEventArgs(removedUser));
            }
        }

        public void AddSubscriber(INotificationSubscriber subscriber)
        {
            if (subscriber == null)
            {
                throw new ArgumentNullException(nameof(subscriber));
            }

            UserAdded += subscriber.UserAdded;
            UserRemoved += subscriber.UserRemoved;
        }

        public void RemoveSubscriber(INotificationSubscriber subscriber)
        {
            if (subscriber == null)
            {
                throw new ArgumentNullException(nameof(subscriber));
            }

            UserAdded -= subscriber.UserAdded;
            UserRemoved -= subscriber.UserRemoved;
        }

        protected virtual void OnUserAdded(UserStorageModifiedEventArgs args)
        {
            UserAdded(this, args);
        }

        protected virtual void OnUserRemoved(UserStorageModifiedEventArgs args)
        {
            UserRemoved(this, args);
        }
    }
}
