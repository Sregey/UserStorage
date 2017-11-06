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
    public class UserMemoryCacheWithState : UserMemoryCache
    {
        private readonly string repositoryFileName;
        private readonly IUserSerializationStrategy userSerializationStrategy;

        public UserMemoryCacheWithState(string repositoryFileName)
        {
            this.repositoryFileName = repositoryFileName;
            this.userSerializationStrategy = new XmlUserSerializationStrategy();
        }

        public override void Start()
        {
            Users = userSerializationStrategy.DeserializeUsers(repositoryFileName);
        }

        public override void Stop()
        {
            userSerializationStrategy.SerializeUsers(repositoryFileName, (List<User>)Users);
        }
    }
}
