using System;
using System.Collections.Generic;

namespace UserStorageServices
{
    public abstract class UserStorageServiceDecorator : IUserStorageService
    {
        protected readonly IUserStorageService UserStorageService;

        protected UserStorageServiceDecorator(IUserStorageService userStorageService)
        {
            UserStorageService = userStorageService;
        }

        public abstract int Count { get; }

        public abstract void Add(User user);

        public abstract void RemoveFirst(Predicate<User> predicate);

        public abstract void RemoveAll(Predicate<User> predicate);

        public abstract IEnumerable<User> Search(Predicate<User> predicate);

        public abstract void UserAdded(User user);

        public abstract void UserRemoved(User user);
    }
}
