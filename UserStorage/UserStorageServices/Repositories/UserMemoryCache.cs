using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorageServices.Repositories
{
    public class UserMemoryCache : IUserRepository
    {
        public int Count { get; }

        public virtual void Start()
        {
        }

        public virtual void Stop()
        {
        }

        public User Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Set()
        {
            throw new NotImplementedException();
        }

        public void Remove()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> Query()
        {
            throw new NotImplementedException();
        }
    }
}
