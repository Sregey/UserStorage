using System;
using UserStorageServices.Notification;
using UserStorageServices.Repositories;
using UserStorageServices.UserStorage;

namespace UserStorageApp
{
    internal class SlaveDomainAction : MarshalByRefObject
    {
        public IUserStorageService SlaveService { get; private set; }

        public INotificationReceiver Receiver { get; private set; }

        public void Run()
        {
            var slave = new UserStorageServiceSlave(new UserMemoryRepository());
            Receiver = slave.Receiver;
            SlaveService = new UserStorageServiceLog(slave);
        }
    }
}
