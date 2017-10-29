using System;
using System.Linq;
using System.ServiceModel;
using UserStorageServices;
using ServiceConfiguration = ServiceConfigurationSection.ServiceConfigurationSection;

namespace UserStorageApp
{
    // In case of AddressAccessDeniedException run the command below as an administrator:
    //   netsh http add urlacl url=<endpoint> user=<username>
    public class Program
    {
        public static void Main(string[] args)
        {
            // Loading configuration from the application configuration file. This configuration is not used yet.
            var serviceConfiguration = (ServiceConfiguration)System.Configuration.ConfigurationManager.GetSection("serviceConfiguration");

            using (var host = new ServiceHost(MyDiagnostics.Create(serviceConfiguration)))
            {
                host.SmartOpen();

                var slaveServices = new IUserStorageService[]
                {
                    new UserStorageServiceLog(new UserStorageService(UserStorageServiceMode.SlaveMode, null)),
                    new UserStorageServiceLog(new UserStorageService(UserStorageServiceMode.SlaveMode, null)),
                };

                var storageService = new UserStorageService(
                    UserStorageServiceMode.MasterMode,
                    slaveServices);
                var client = new Client(new UserStorageServiceLog(storageService));

                client.Run();

                Console.WriteLine("Service \"{0}\" that is implemented by \"{1}\" class is available on \"{2}\" endpoint.", host.Description.Name, host.Description.ServiceType.FullName, host.BaseAddresses.First());
                Console.WriteLine("Press <Enter> to stop the service.");
                Console.ReadLine();

                host.Close();
            }
        }
    }
}
