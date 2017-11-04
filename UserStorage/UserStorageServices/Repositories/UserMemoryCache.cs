using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorageServices.Repositories
{
    public class UserMemoryCache : IUserRepository
    {
        public UserMemoryCache()
        {
            Users = new List<User>();
        }

        protected IList<User> Users { get; set; }

        public int Count => Users.Count;

        public virtual void Start()
        {
        }

        public virtual void Stop()
        {
        }

        public User Get(Guid id)
        {
            return Users.FirstOrDefault((u) => u.Id == id);
        }

        public void Set(User user)
        {
            Users.Add(user);
        }

        public User Delete(Predicate<User> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            int i;
            for (i = 0; i < this.Count; i++)
            {
                if (predicate(this.Users[i]))
                {
                    break;
                }
            }

            if (i < Users.Count)
            {
                var removedUser = Users[i];
                Users.RemoveAt(i);
                return removedUser;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<User> Query(Predicate<User> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return Users.Where((u) => predicate(u));
        }
    }
}
