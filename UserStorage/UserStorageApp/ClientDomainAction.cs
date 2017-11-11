using System;

namespace UserStorageApp
{
    internal class ClientDomainAction : MarshalByRefObject
    {
        public Client Client { get; private set; }

        public void Run()
        {
            var masterDomain = AppDomain.CreateDomain("MasterDomain");

            var masterType = typeof(MasterDomainAction);
            var masterAction = (MasterDomainAction)masterDomain.CreateInstanceAndUnwrap(
                masterType.Assembly.FullName, 
                masterType.FullName);

            masterAction.Run();
            Client = new Client(masterAction.MasterService, masterAction.RepositoryManager);
        }
    }
}
