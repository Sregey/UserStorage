using System;
using System.Collections.Generic;

namespace UserStorageServices.UserStorage
{
    public abstract class UserStorageServiceDecorator : MarshalByRefObject, IUserStorageService
    {
        protected readonly IUserStorageService UserStorageService;

        protected UserStorageServiceDecorator(IUserStorageService userStorageService)
        {
            UserStorageService = userStorageService;
        }

        public abstract int Count { get; }

        public abstract void Add(User user);

        public abstract void Remove(Predicate<User> predicate);

        public abstract IEnumerable<User> Search(Predicate<User> predicate);
    }
}
