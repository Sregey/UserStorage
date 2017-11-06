using System;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using UserStorageServices.Notification;
using UserStorageServices.Repositories;
using UserStorageServices.UserStorage;
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
            var serviceConfiguration = (ServiceConfiguration)ConfigurationManager.GetSection("serviceConfiguration");

            using (var host = new ServiceHost(MyDiagnostics.Create(serviceConfiguration)))
            {
                host.SmartOpen();

                INotificationReceiver receiver = new NotificationReceiver();

                var slaveServices = new IUserStorageService[]
                {
                    new UserStorageServiceLog(new UserStorageServiceSlave(new UserMemoryRepository(), receiver)),
                    new UserStorageServiceLog(new UserStorageServiceSlave(new UserMemoryRepository(), receiver)),
                };

                var repositoryFileName = ConfigurationManager.AppSettings["UserMemoryCacheWithStateFileName"];
                var userRepositoryForMaster = new UserDiskRepository(repositoryFileName);
                var storageService = new UserStorageServiceMaster(userRepositoryForMaster, receiver);
                var client = new Client(new UserStorageServiceLog(storageService), userRepositoryForMaster);

                client.Run();

                Console.WriteLine("Service \"{0}\" that is implemented by \"{1}\" class is available on \"{2}\" endpoint.", host.Description.Name, host.Description.ServiceType.FullName, host.BaseAddresses.First());
                Console.WriteLine("Press <Enter> to stop the service.");
                Console.ReadLine();

                host.Close();
            }
        }
    }
}
