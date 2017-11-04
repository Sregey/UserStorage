using System;
using System.Collections.Generic;

namespace UserStorageServices.Repositories
{
    public interface IUserRepository
    {
        int Count { get; }

        void Start();

        void Stop();

        User Get(Guid id);

        void Set();

        void Remove();

        IEnumerable<User> Query();
    }
}
