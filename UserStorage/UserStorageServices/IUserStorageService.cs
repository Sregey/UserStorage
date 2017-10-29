using System;
using System.Collections.Generic;

namespace UserStorageServices
{
    public interface IUserStorageService : INotificationSubscriber
    {
        int Count { get; }

        void Add(User user);

        void RemoveFirst(Predicate<User> predicate);

        void RemoveAll(Predicate<User> predicate);

        IEnumerable<User> Search(Predicate<User> predicate);
    }
}
