using System;
using System.Linq;
using System.Reflection;
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
            Assembly assembly = Assembly.Load("UserStorageServices, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f46a87b3d9a80705");

            var slaveServiceType = assembly.ExportedTypes
                .Where(t => t.GetCustomAttribute<MyApplicationServiceAttribute>() != null)
                .First(t => t.GetCustomAttribute<MyApplicationServiceAttribute>().ServiceMode == "UserStorageSlave");

            var slave = (UserStorageServiceSlave)Activator.CreateInstance(
                slaveServiceType,
                new UserMemoryRepository());
            Receiver = slave.Receiver;
            SlaveService = new UserStorageServiceLog(slave);
        }
    }
}
