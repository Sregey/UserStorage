using System.Collections.Generic;

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
            LastGuid = dataSet.LastId;
        }

        public override void Stop()
        {
            userSerializationStrategy.SerializeUsers(repositoryFileName, new DataSetForUserRepository((List<User>)Users, LastGuid));
        }
    }
}
