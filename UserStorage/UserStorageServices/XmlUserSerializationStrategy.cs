using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UserStorageServices
{
    public class XmlUserSerializationStrategy : IUserSerializationStrategy
    {
        public void SerializeUsers(string repositoryFileName, List<User> users)
        {
            if (string.IsNullOrWhiteSpace(repositoryFileName))
            {
                throw new ArgumentException(nameof(repositoryFileName));
            }

            if (users == null)
            {
                throw new ArgumentNullException(nameof(users));
            }

            XmlSerializer formatter = new XmlSerializer(typeof(List<User>));
            
            using (FileStream fs = new FileStream(repositoryFileName, FileMode.Create))
            {
                formatter.Serialize(fs, users);
            }
        }

        public List<User> DeserializeUsers(string repositoryFileName)
        {
            if (!File.Exists(repositoryFileName))
            {
                return new List<User>();
            }

            XmlSerializer formatter = new XmlSerializer(typeof(List<User>));

            using (FileStream fs = new FileStream(repositoryFileName, FileMode.Open))
            {
                return (List<User>)formatter.Deserialize(fs);
            }
        }
    }
}
