using System;
using System.Collections.Generic;

namespace UserStorageServices.UserStorage
{
    public interface IUserStorageService
    {
        int Count { get; }

        void Add(User user);

        void Remove(Predicate<User> predicate);

        IEnumerable<User> Search(Predicate<User> predicate);
    }
}
