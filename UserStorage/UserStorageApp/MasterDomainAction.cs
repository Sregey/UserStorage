using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
            
            var storageService = new UserStorageServiceMaster(repository, slaveActions.Select(s => s.Receiver));
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
