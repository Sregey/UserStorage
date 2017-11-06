using System;
using System.Collections.Generic;

namespace UserStorageServices.Repositories
{
    public interface IUserRepository
    {
        int Count { get; }

        Guid LastId { get; }

        User Get(Guid id);

        void Set(User user);

        User Delete(Predicate<User> predicate);

        IEnumerable<User> Query(Predicate<User> predicate);
    }
}
