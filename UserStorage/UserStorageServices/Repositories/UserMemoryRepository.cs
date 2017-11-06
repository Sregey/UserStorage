using System;
using System.Collections.Generic;
using System.Linq;
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

        public int Count => Users.Count;

        public Guid LastId => LastGuid;

        protected Guid LastGuid { get; set; }

        protected IList<User> Users { get; set; }

        public User Get(Guid id)
        {
            return Users.FirstOrDefault((u) => u.Id == id);
        }

        public void Set(User user)
        {
            userValidator.Validate(user);
            LastGuid = user.Id;

            Users.Add(user);
        }

        public User Delete(Predicate<User> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            int i;
            for (i = 0; i < Count; i++)
            {
                if (predicate(Users[i]))
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
