using System;
using System.Collections.Generic;
using System.Linq;

namespace UserStorageServices
{
    /// <summary>
    /// Represents a service that stores a set of <see cref="User"/>s and allows to search through them.
    /// </summary>
    public class UserStorageService
    {
        private List<User> users;

        private IIdGenerator idGenerator;
        private IUserValidator userValidator;

        public UserStorageService()
        {
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
        /// Determines is logging enabled. 
        /// </summary>
        public bool IsLoggingEnabled { get; set; }

        /// <summary>
        /// Adds a new <see cref="User"/> to the storage.
        /// </summary>
        /// <param name="user">A new <see cref="User"/> that will be added to the storage.</param>
        public void Add(User user)
        {
            userValidator.Validate(user);

            user.Id = idGenerator.Generate();
            users.Add(user);

            Log("Add() method is called.");
        }

        /// <summary>
        /// Removes first <see cref="User"/> from the storage by predicate.
        /// </summary>
        public void RemoveFirst(Predicate<User> predicate)
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

            Log("RemoveFirst() method is called.");
        }

        /// <summary>
        /// Removes all <see cref="User"/> from the storage by predicate.
        /// </summary>
        public void RemoveAll(Predicate<User> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            users.RemoveAll(predicate);

            Log("RemoveAll() method is called.");
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

            Log("Search() method is called.");

            return users.Where((u) => predicate(u));
        }

        private void Log(string message)
        {
            if (IsLoggingEnabled)
            {
                Console.WriteLine(message);
            }
        }
    }
}
