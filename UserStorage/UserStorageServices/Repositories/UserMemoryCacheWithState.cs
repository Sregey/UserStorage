using System;
using System.Collections.Generic;
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
        public override void Start()
        {
            FileStream fs = new FileStream("repository.bin", FileMode.Open);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();

                // Deserialize the hashtable from the file and 
                // assign the reference to the local variable.
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
            FileStream fs = new FileStream("repository.bin", FileMode.Create);
            
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
