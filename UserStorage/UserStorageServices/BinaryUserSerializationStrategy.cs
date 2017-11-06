using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace UserStorageServices
{
    public class BinaryUserSerializationStrategy : IUserSerializationStrategy
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

            FileStream fs = new FileStream(repositoryFileName, FileMode.Create);

            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, users);
            }
            catch (SerializationException e)
            {
                Trace.WriteLine("Failed to serialize users in UserMemoryCacheWithState. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Dispose();
            }
        }

        public List<User> DeserializeUsers(string repositoryFileName)
        {
            if (!File.Exists(repositoryFileName))
            {
                return new List<User>();
            }

            FileStream fs = new FileStream(repositoryFileName, FileMode.Open);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();

                return (List<User>)formatter.Deserialize(fs);
            }
            catch (SerializationException e)
            {
                Trace.WriteLine("Failed to deserialize users in UserMemoryCacheWithState. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Dispose();
            }
        }
    }
}
