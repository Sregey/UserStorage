using System;

namespace UserStorageApp
{
    class ClientDomainAction : MarshalByRefObject
    {
        public Client Client { get; private set; }

        public void Run()
        {
            var masterDomain = AppDomain.CreateDomain("MasterDomain");

            var masterType = typeof(MasterDomainAction);
            var masterAction = (MasterDomainAction)masterDomain.CreateInstanceAndUnwrap(
                masterType.Assembly.FullName, 
                masterType.FullName);

            masterAction.Run(2);
            Client = new Client(masterAction.MasterService, masterAction.RepositoryManager);
        }
    }
}
