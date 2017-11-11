using System;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using ServiceConfiguration = ServiceConfigurationSection.ServiceConfigurationSection;

namespace UserStorageApp
{
    // In case of AddressAccessDeniedException run the command below as an administrator:
    //   netsh http add urlacl url=<endpoint> user=<username>
    public class Program
    {
        public static void Main(string[] args)
        {
            var serviceConfiguration = (ServiceConfiguration)ConfigurationManager.GetSection("serviceConfiguration");

            using (var host = new ServiceHost(MyDiagnostics.Create(serviceConfiguration)))
            {
                host.SmartOpen();

                var clientDomain = AppDomain.CreateDomain("ClientDomain");

                var clientType = typeof(ClientDomainAction);
                var clientAction = (ClientDomainAction)clientDomain.CreateInstanceAndUnwrap(
                    clientType.Assembly.FullName,
                    clientType.FullName);

                clientAction.Run();
                var client = clientAction.Client;

                client.Run();

                Console.WriteLine("Service \"{0}\" that is implemented by \"{1}\" class is available on \"{2}\" endpoint.", host.Description.Name, host.Description.ServiceType.FullName, host.BaseAddresses.First());
                Console.WriteLine("Press <Enter> to stop the service.");
                Console.ReadLine();

                host.Close();
            }
        }
    }
}
