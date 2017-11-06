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
        public void SerializeUsers(string repositoryFileName, DataSetForUserRepository dataSet)
        {
            if (string.IsNullOrWhiteSpace(repositoryFileName))
            {
                throw new ArgumentException(nameof(repositoryFileName));
            }

            if (dataSet == null)
            {
                throw new ArgumentNullException(nameof(dataSet));
            }

            if (dataSet.Users == null)
            {
                throw new ArgumentNullException(nameof(dataSet.Users));
            }

            XmlSerializer formatter = new XmlSerializer(typeof(DataSetForUserRepository));

            using (FileStream fs = new FileStream(repositoryFileName, FileMode.Create))
            {
                formatter.Serialize(fs, dataSet);
            }
        }

        public DataSetForUserRepository DeserializeUsers(string repositoryFileName)
        {
            if (!File.Exists(repositoryFileName))
            {
                return new DataSetForUserRepository(new List<User>(), new Guid());
            }

            XmlSerializer formatter = new XmlSerializer(typeof(DataSetForUserRepository));

            using (FileStream fs = new FileStream(repositoryFileName, FileMode.Open))
            {
                return (DataSetForUserRepository)formatter.Deserialize(fs);
            }
        }
    }
}
