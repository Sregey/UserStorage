using System;
using System.Collections.Generic;
using System.Linq;
using UserStorageServices.Validation;

namespace UserStorageServices
{
    /// <summary>
    /// Represents a service that stores a set of <see cref="User"/>s and allows to search through them.
    /// </summary>
    public class UserStorageService : IUserStorageService
    {
        private const string NOT_SUPPORTED_MESSAGE = "This service is slave";

        private UserStorageServiceMode mode;
        private IList<INotificationSubscriber> subscribers;

        private List<User> users;

        private IIdGenerator idGenerator;
        private IValidator<User> userValidator;

        public UserStorageService(UserStorageServiceMode mode, IEnumerable<INotificationSubscriber> slaveServices)
        {
            this.mode = mode;
            if (mode == UserStorageServiceMode.MasterMode)
            {
                slaveServices = slaveServices ?? Enumerable.Empty<INotificationSubscriber>();
                subscribers = slaveServices.ToList() ;
            }

            users = new List<User>();
            idGenerator = new IdGenerator();
            userValidator = new UserValidator();
        }

        /// <summary>
        /// Gets the number of elements contained in the storage.
        /// </summary>
        /// <returns>An amount of users in the storage.</returns>
        public int Count => users.Count;

        /// <summary>
        /// Adds a new <see cref="User"/> to the storage.
        /// </summary>
        /// <param name="user">A new <see cref="User"/> that will be added to the storage.</param>
        public void Add(User user)
        {
            if (mode == UserStorageServiceMode.MasterMode)
            {
                userValidator.Validate(user);

                user.Id = idGenerator.Generate();
                users.Add(user);

                foreach (var subscriber in subscribers)
                {
                    subscriber.UserAdded(user);
                }
            }
            else
            {
                throw new NotSupportedException(NOT_SUPPORTED_MESSAGE);
            }
        }

        /// <summary>
        /// Removes first <see cref="User"/> from the storage by predicate.
        /// </summary>
        public void RemoveFirst(Predicate<User> predicate)
        {
            if (mode == UserStorageServiceMode.MasterMode)
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
                    users.RemoveAt(i);
                }

                foreach (var subscriber in subscribers)
                {
                    subscriber.UserRemoved(users[i]);
                }
            }
            else
            {
                throw new NotSupportedException(NOT_SUPPORTED_MESSAGE);
            }
        }

        /// <summary>
        /// Removes all <see cref="User"/> from the storage by predicate.
        /// </summary>
        public void RemoveAll(Predicate<User> predicate)
        {
            if (mode == UserStorageServiceMode.MasterMode)
            {
                if (predicate == null)
                {
                    throw new ArgumentNullException(nameof(predicate));
                }

                var removedUsers = Search(predicate);
                users.RemoveAll(predicate);

                foreach (var subscriber in subscribers)
                {
                    foreach (var removedUser in removedUsers)
                    {
                        subscriber.UserRemoved(removedUser);
                    }
                }
            }
            else
            {
                throw new NotSupportedException(NOT_SUPPORTED_MESSAGE);
            }
        }

        /// <summary>
        /// Searches through the storage for a <see cref="User"/> that matches specified criteria.
        /// </summary>
        public IEnumerable<User> Search(Predicate<User> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return users.Where((u) => predicate(u));
        }

        public void UserAdded(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            users.Add(user);
        }

        public void UserRemoved(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            users.Remove(user);
        }

        public void AddSubscriber(INotificationSubscriber subscriber)
        {
            if (subscriber == null)
            {
                throw new ArgumentNullException(nameof(subscriber));
            }

            subscribers.Add(subscriber);
        }

        public void RemoveSubscriber(INotificationSubscriber subscriber)
        {
            if (subscriber == null)
            {
                throw new ArgumentNullException(nameof(subscriber));
            }

            subscribers.Remove(subscriber);
        }
    }
}
