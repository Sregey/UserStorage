using System;

namespace UserStorageServices
{
    class IdGenerator : IIdGenerator
    {
        public Guid Generate()
        {
            return Guid.NewGuid();
        }
    }
}
