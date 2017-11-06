using NUnit.Framework;
using UserStorageServices.Notification;
using UserStorageServices.Repositories;
using UserStorageServices.UserStorage;

namespace UserStorageServices.Tests
{
    using static UserTestHealper;

    class MasterAndSlaveInteractionTests
    {
        [Test]
        public void AddUsersToMaster_SameUsersAppearInSlaves()
        {
            // Arrange
            INotificationReceiver receiver = new NotificationReceiver();

            var slaveServices = new IUserStorageService[]
            {
                new UserStorageServiceLog(new UserStorageServiceSlave(new UserMemoryRepository(), receiver)),
                new UserStorageServiceLog(new UserStorageServiceSlave(new UserMemoryRepository(), receiver)),
            };
            
            var masterService = new UserStorageServiceMaster(new UserMemoryRepository(), receiver);

            // Act
            InitUserStoreageService(masterService);
            var slaveUsers0 = slaveServices[0].Search((u) => true);
            var slaveUsers1 = slaveServices[1].Search((u) => true);

            // Assert
            AssertAreEqual(Users, slaveUsers0, CompareUsers);
            AssertAreEqual(Users, slaveUsers1, CompareUsers);
        }

        [Test]
        public void RemoveUsersToMaster_SameUsersRemoveInSlaves()
        {
            // Arrange
            INotificationReceiver receiver = new NotificationReceiver();

            var slaveServices = new IUserStorageService[]
            {
                new UserStorageServiceLog(new UserStorageServiceSlave(new UserMemoryRepository(), receiver)),
                new UserStorageServiceLog(new UserStorageServiceSlave(new UserMemoryRepository(), receiver)),
            };

            var masterService = new UserStorageServiceMaster(new UserMemoryRepository(), receiver);
            InitUserStoreageService(masterService);

            // Act
            masterService.Remove((u) => u.Age > 12);
            var slaveUsers0 = slaveServices[0].Search((u) => true);
            var slaveUsers1 = slaveServices[1].Search((u) => true);
            var masterUsers = masterService.Search((u) => true);

            // Assert
            AssertAreEqual(masterUsers, slaveUsers0, CompareUsers);
            AssertAreEqual(masterUsers, slaveUsers1, CompareUsers);
        }
    }
}
