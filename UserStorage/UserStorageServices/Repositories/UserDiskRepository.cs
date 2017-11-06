using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace UserStorageServices.Repositories
{
    public class UserDiskRepository : UserMemoryRepository
    {
        private readonly string repositoryFileName;
        private readonly IUserSerializationStrategy userSerializationStrategy;

        public UserDiskRepository(string repositoryFileName)
        {
            this.repositoryFileName = repositoryFileName;
            this.userSerializationStrategy = new BinaryUserSerializationStrategy();
            Start();
        }

        public override void Start()
        {
            var dataSet = userSerializationStrategy.DeserializeUsers(repositoryFileName);
            Users = dataSet.Users;
            lastId = dataSet.LastId;
        }

        public override void Stop()
        {
            userSerializationStrategy.SerializeUsers(repositoryFileName, new DataSetForUserRepository((List<User>)Users, lastId));
        }
    }
}
