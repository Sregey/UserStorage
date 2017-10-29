using System;

namespace UserStorageServices
{
    internal class IdGenerator : IIdGenerator
    {
        public Guid Generate()
        {
            return Guid.NewGuid();
        }
    }
}
