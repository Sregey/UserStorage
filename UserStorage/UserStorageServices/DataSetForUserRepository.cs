using System;
using System.Collections.Generic;

namespace UserStorageServices
{
    [Serializable]
    public class DataSetForUserRepository
    {
        public List<User> Users { get; set; }

        public Guid LastId { get; set; }

        public DataSetForUserRepository()
        {
        }

        public DataSetForUserRepository(List<User> users, Guid lastId)
        {
            Users = users;
            LastId = lastId;
        }
    }
}
