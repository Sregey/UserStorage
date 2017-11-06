using System;
using System.Collections.Generic;
using System.Linq;
using UserStorageServices.Repositories;
using UserStorageServices.Validation;

namespace UserStorageServices
{
    public class UserStorageServiceMaster : UserStorageServiceBase
    {
        private readonly IIdGenerator idGenerator;
        private readonly IValidator<User> userValidator;

        public UserStorageServiceMaster(IUserRepository userRepository, IEnumerable<INotificationSubscriber> slaveServices)
            : base(userRepository)
        {
            slaveServices = slaveServices ?? Enumerable.Empty<INotificationSubscriber>();
            foreach (var subscriber in slaveServices)
            {
                AddSubscriber(subscriber);
            }

            idGenerator = new IdGenerator(userRepository.LastId);
            userValidator = new UserValidator();
        }

        private event EventHandler<UserStorageModifiedEventArgs> UserAdded = delegate { };

        private event EventHandler<UserStorageModifiedEventArgs> UserRemoved = delegate { };

        public override UserStorageServiceMode ServiceMode => UserStorageServiceMode.MasterMode;

        public override void Add(User user)
        {
            userValidator.Validate(user);
            user.Id = idGenerator.Generate();
            UserRepository.Set(user);

            OnUserAdded(new UserStorageModifiedEventArgs(user));
        }

        public override void Remove(Predicate<User> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            var removedUser = UserRepository.Delete(predicate);
            if (removedUser != null)
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
