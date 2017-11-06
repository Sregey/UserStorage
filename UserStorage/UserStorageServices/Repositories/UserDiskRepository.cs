using System.Collections.Generic;

namespace UserStorageServices.Repositories
{
    public class UserDiskRepository : UserMemoryRepository, IUserRepositoryManager
    {
        private readonly string repositoryFileName;
        private readonly IUserSerializationStrategy userSerializationStrategy;

        public UserDiskRepository(string repositoryFileName)
        {
            this.repositoryFileName = repositoryFileName;
            this.userSerializationStrategy = new BinaryUserSerializationStrategy();
            Start();
        }

        public void Start()
        {
            var dataSet = userSerializationStrategy.DeserializeUsers(repositoryFileName);
            Users = dataSet.Users;
            LastGuid = dataSet.LastId;
        }

        public void Stop()
        {
            userSerializationStrategy.SerializeUsers(repositoryFileName, new DataSetForUserRepository((List<User>)Users, LastGuid));
        }
    }
}
