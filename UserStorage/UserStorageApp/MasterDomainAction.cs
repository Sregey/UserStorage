using System;
using System.Configuration;
using System.Linq;
using UserStorageServices.Repositories;
using UserStorageServices.UserStorage;

namespace UserStorageApp
{
    class MasterDomainAction : MarshalByRefObject
    {
        private SlaveDomainAction[] slaveActions;

        public IUserRepositoryManager RepositoryManager { get; private set; }

        public IUserStorageService MasterService { get; private set; }

        public IUserStorageService this[int i] => slaveActions[i].SlaveService;

        public void Run(int countOfSlaveServices)
        {
            RunSlaves(countOfSlaveServices);

            var repositoryFileName = ConfigurationManager.AppSettings["UserMemoryCacheWithStateFileName"];
            var repository = new UserDiskRepository(repositoryFileName);
            RepositoryManager = repository;
            
            var storageService = new UserStorageServiceMaster(repository, slaveActions.Select(s => s.Receiver));
            MasterService = new UserStorageServiceLog(storageService);
        }

        private void RunSlaves(int count)
        {
            slaveActions = new SlaveDomainAction[count];
            for (int i = 0; i < count; i++)
            {
                var slaveDomain = AppDomain.CreateDomain("SlaveDomain_" + i);

                var slvaeType = typeof(SlaveDomainAction);
                slaveActions[i] = (SlaveDomainAction)slaveDomain.CreateInstanceAndUnwrap(
                    slvaeType.Assembly.FullName,
                    slvaeType.FullName);
                slaveActions[i].Run();
            }
        }
    }
}
