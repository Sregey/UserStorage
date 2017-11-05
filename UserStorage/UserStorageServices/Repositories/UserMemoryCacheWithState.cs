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

        public UserMemoryCacheWithState(string repositoryFileName)
        {
            this.repositoryFileName = repositoryFileName;
        }

        public override void Start()
        {
            if (!File.Exists(repositoryFileName))
            {
                Users = new List<User>();
                return;
            }

            FileStream fs = new FileStream(repositoryFileName, FileMode.Open);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                
                Users = (List<User>)formatter.Deserialize(fs);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize users in UserMemoryCacheWithState. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Dispose();
            }
        }

        public override void Stop()
        {
            FileStream fs = new FileStream(repositoryFileName, FileMode.Create);
            
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, Users);
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
    }
}
