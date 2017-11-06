using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorageServices.Validation;

namespace UserStorageServices.Repositories
{
    public class UserMemoryRepository : IUserRepository
    {
        private readonly UserValidator userValidator;

        public UserMemoryRepository()
        {
            Users = new List<User>();
            userValidator = new UserValidator();
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
            this.userValidator.Validate(user);

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
