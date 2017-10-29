using System;

namespace UserStorageServices
{
    internal interface IIdGenerator
    {
        Guid Generate();
    }
}
