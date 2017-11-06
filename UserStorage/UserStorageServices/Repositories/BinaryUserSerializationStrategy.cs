using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace UserStorageServices.Repositories
{
    public class BinaryUserSerializationStrategy : IUserSerializationStrategy
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

            FileStream fs = new FileStream(repositoryFileName, FileMode.Create);

            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, dataSet);
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

        public DataSetForUserRepository DeserializeUsers(string repositoryFileName)
        {
            if (!File.Exists(repositoryFileName))
            {
                return new DataSetForUserRepository(new List<User>(), Guid.Empty);
            }

            FileStream fs = new FileStream(repositoryFileName, FileMode.Open);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();

                return (DataSetForUserRepository)formatter.Deserialize(fs);
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
