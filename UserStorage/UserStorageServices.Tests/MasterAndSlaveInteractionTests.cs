using System.Linq;
using NUnit.Framework;
using UserStorageServices.Repositories;
using UserStorageServices.UserStorage;

namespace UserStorageServices.Tests
{
    using static UserTestHealper;

    internal class MasterAndSlaveInteractionTests
    {
        [Test]
        public void AddUsersToMaster_SameUsersAppearInSlaves()
        {
            // Arrange
            var slaveServices = new[]
            {
                new UserStorageServiceSlave(new UserMemoryRepository()),
                new UserStorageServiceSlave(new UserMemoryRepository()),
            };

            var receivers = slaveServices.Select(s => s.Receiver);
            var masterService = new UserStorageServiceMaster(new UserMemoryRepository(), receivers);

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
            var slaveServices = new[]
            {
                new UserStorageServiceSlave(new UserMemoryRepository()),
                new UserStorageServiceSlave(new UserMemoryRepository()),
            };

            var receivers = slaveServices.Select(s => s.Receiver);
            var masterService = new UserStorageServiceMaster(new UserMemoryRepository(), receivers);
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
