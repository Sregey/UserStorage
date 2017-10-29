using System;
using System.Collections.Generic;
using System.Linq;

namespace UserStorageServices
{
    /// <summary>
    /// Represents a service that stores a set of <see cref="User"/>s and allows to search through them.
    /// </summary>
    public abstract class UserStorageServiceBase : IUserStorageService
    { 
        protected List<User> users;

        public UserStorageServiceBase()
        {
            users = new List<User>();
        }

        /// <summary>
        /// Gets the mode of user storage service.
        /// </summary>
        /// <returns>Mode of user storage service.</returns>
        public abstract UserStorageServiceMode ServiceMode { get; }

        /// <summary>
        /// Gets the number of elements contained in the storage.
        /// </summary>
        /// <returns>An amount of users in the storage.</returns>
        public int Count => users.Count;

        /// <summary>
        /// Adds a new <see cref="User"/> to the storage.
        /// </summary>
        /// <param name="user">A new <see cref="User"/> that will be added to the storage.</param>
        public abstract void Add(User user);

        /// <summary>
        /// Removes first <see cref="User"/> from the storage by predicate.
        /// </summary>
        public abstract void RemoveFirst(Predicate<User> predicate);

        /// <summary>
        /// Removes all <see cref="User"/> from the storage by predicate.
        /// </summary>
        public abstract void RemoveAll(Predicate<User> predicate);

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
    }
}
