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
                this.AddSubscriber(subscriber);
            }

            this.idGenerator = new IdGenerator();
            this.userValidator = new UserValidator();
        }

        private event EventHandler<UserStorageModifiedEventArgs> UserAdded = delegate { };

        private event EventHandler<UserStorageModifiedEventArgs> UserRemoved = delegate { };

        public override UserStorageServiceMode ServiceMode => UserStorageServiceMode.MasterMode;

        public override void Add(User user)
        {
            this.userValidator.Validate(user);

            user.Id = this.idGenerator.Generate();
            Users.Add(user);

            this.OnUserAdded(new UserStorageModifiedEventArgs(user));
        }

        public override void RemoveFirst(Predicate<User> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            int i;
            for (i = 0; i < this.Count; i++)
            {
                if (predicate(this.Users[i]))
                {
                    break;
                }
            }

            if (i < Users.Count)
            {
                var removedUser = Users[i];
                Users.RemoveAt(i);
                this.OnUserRemoved(new UserStorageModifiedEventArgs(removedUser));
            }
        }

        public override void RemoveAll(Predicate<User> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            var removedUsers = Search(predicate);
            Users.RemoveAll(predicate);

            foreach (var removedUser in removedUsers)
            {
                this.OnUserRemoved(new UserStorageModifiedEventArgs(removedUser));
            }
        }

        public void AddSubscriber(INotificationSubscriber subscriber)
        {
            if (subscriber == null)
            {
                throw new ArgumentNullException(nameof(subscriber));
            }

            this.UserAdded += subscriber.UserAdded;
            this.UserRemoved += subscriber.UserRemoved;
        }

        public void RemoveSubscriber(INotificationSubscriber subscriber)
        {
            if (subscriber == null)
            {
                throw new ArgumentNullException(nameof(subscriber));
            }

            this.UserAdded -= subscriber.UserAdded;
            this.UserRemoved -= subscriber.UserRemoved;
        }

        protected virtual void OnUserAdded(UserStorageModifiedEventArgs args)
        {
            this.UserAdded(this, args);
        }

        protected virtual void OnUserRemoved(UserStorageModifiedEventArgs args)
        {
            this.UserRemoved(this, args);
        }
    }
}
