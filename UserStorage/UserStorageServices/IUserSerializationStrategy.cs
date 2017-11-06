using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorageServices
{
    public interface IUserSerializationStrategy
    {
        void SerializeUsers(string repositoryFileName, List<User> users);

        List<User> DeserializeUsers(string repositoryFileName);
    }
}
