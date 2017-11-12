using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using ServiceConfigurationSection;
using UserStorageServices.Repositories;
using UserStorageServices.UserStorage;
using ServiceConfiguration = ServiceConfigurationSection.ServiceConfigurationSection;

namespace UserStorageApp
{
    internal class MasterDomainAction : MarshalByRefObject
    {
        private List<SlaveDomainAction> slaveActions;

        public IUserRepositoryManager RepositoryManager { get; private set; }

        public IUserStorageService MasterService { get; private set; }

        public IUserStorageService this[int i] => slaveActions[i].SlaveService;

        public void Run()
        {
           RunSlaves();

            var repositoryFileName = ConfigurationManager.AppSettings["UserMemoryCacheWithStateFileName"];
            var repository = new UserDiskRepository(repositoryFileName);
            RepositoryManager = repository;

            Assembly assembly = Assembly.Load("UserStorageServices, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f46a87b3d9a80705");

            var masterServiceType = assembly.ExportedTypes
                .Where(t => t.GetCustomAttribute<MyApplicationServiceAttribute>() != null)
                .First(t => t.GetCustomAttribute<MyApplicationServiceAttribute>().ServiceMode == "UserStorageMaster");

            var storageService = (IUserStorageService)Activator.CreateInstance(
                masterServiceType, 
                repository, 
                slaveActions.Select(s => s.Receiver));
            MasterService = new UserStorageServiceLog(storageService);
        }

        private void RunSlaves()
        {
            var serviceConfiguration = (ServiceConfiguration)ConfigurationManager.GetSection("serviceConfiguration");

            slaveActions = new List<SlaveDomainAction>();
            foreach (var serviceInstance in serviceConfiguration.ServiceInstances)
            {
                if (serviceInstance.Mode == ServiceInstanceMode.Slave)
                {
                    var slaveDomain = AppDomain.CreateDomain("SlaveDomain_" + slaveActions.Count);

                    var slvaeType = typeof(SlaveDomainAction);
                    var slaveAction = (SlaveDomainAction)slaveDomain.CreateInstanceAndUnwrap(
                        slvaeType.Assembly.FullName,
                        slvaeType.FullName);
                    slaveAction.Run();

                    slaveActions.Add(slaveAction);
                }
            }
        }
    }
}
