using System;

namespace UserStorageServices
{
    interface IIdGenerator
    {
        Guid Generate();
    }
}
