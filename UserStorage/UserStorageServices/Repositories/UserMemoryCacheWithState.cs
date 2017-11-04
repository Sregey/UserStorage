using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorageServices.Repositories
{
    public class UserMemoryCacheWithState : UserMemoryCache
    {
        public override void Start()
        {
            //load from file
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            //load to file
            throw new NotImplementedException();
        }
    }
}
