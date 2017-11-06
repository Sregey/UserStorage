using System;

namespace UserStorageServices.UserStorage
{
    internal interface IIdGenerator
    {
        Guid Generate();
    }
}
