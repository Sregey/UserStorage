using System;
using System.Collections.Generic;

namespace UserStorageServices.Repositories
{
    [Serializable]
    public class DataSetForUserRepository
    {
        public DataSetForUserRepository()
        {
        }

        public DataSetForUserRepository(List<User> users, Guid lastId)
        {
            Users = users;
            LastId = lastId;
        }

        public List<User> Users { get; set; }

        public Guid LastId { get; set; }
    }
}
