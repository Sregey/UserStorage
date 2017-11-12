using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UserStorageServices.Notification;
using UserStorageServices.Repositories;
using UserStorageServices.UserStorage;

namespace UserStorageServices.Tests
{
    using static UserTestHealper;

    internal class UserStorageServiceTests
    {
        public static IEnumerable<TestCaseData> InvalidUsers => UserTestHealper.InvalidUsers;

        public IEnumerable<TestCaseData> UsersInStorage => UserTestHealper.UsersInRepository;

        public IEnumerable<TestCaseData> SearchPredicates => UserTestHealper.SearchPredicates;

        [Test, TestCaseSource(nameof(InvalidUsers))]
        public void Add_InvalidUser_ExceptionThrown(User user)
        {
            // Arrange
            var userStorageService = new UserStorageServiceMaster(
                new UserMemoryRepository(),
                Enumerable.Empty<INotificationReceiver>());

            // Act
            userStorageService.Add(user);
        }

        [Test]
        public void Add_ValidUser_StorageCountIs1()
        {
            // Arrange
            var userStorageService = new UserStorageServiceMaster(
                new UserMemoryRepository(),
                Enumerable.Empty<INotificationReceiver>());
            var user = GetValidUser();

            // Act
            userStorageService.Add(user);

            // Assert
            Assert.AreEqual(1, userStorageService.Count);
        }

        [Test]
        public void Add_ValidUserToSlaveService_ExceptionThrown()
        {
            // Arrange
            var userStorageService = new UserStorageServiceSlave(new UserMemoryRepository());
            var user = GetValidUser();

            // Assert
            Assert.Throws<NotSupportedException>(() => userStorageService.Add(user));
        }

        [Test, TestCaseSource(nameof(UsersInStorage))]
        public void Remove_ExistingUser_RemoveOneUser(User user)
        {
            // Arrange
            var userStorageService = new UserStorageServiceMaster(
                new UserMemoryRepository(),
                Enumerable.Empty<INotificationReceiver>());
            InitUserStoreageService(userStorageService);
            int oldUserCount = userStorageService.Count;

            // Act
            userStorageService.Remove((u) => u == user);

            // Assert
            Assert.AreEqual(oldUserCount - 1, userStorageService.Count);
        }

        [Test]
        public void Remove_NotExistingUser_RemoveNoUsers()
        {
            // Arrange
            var userStorageService = new UserStorageServiceMaster(
                new UserMemoryRepository(),
                Enumerable.Empty<INotificationReceiver>());
            InitUserStoreageService(userStorageService);
            int oldUserCount = userStorageService.Count;
            var user = GetValidUser();

            // Act
            userStorageService.Remove((u) => u == user);

            // Assert
            Assert.AreEqual(oldUserCount, userStorageService.Count);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        public void Remove_InvalidPredicate_ExceptionThrown(Predicate<User> predicate)
        {
            // Arrange
            var userStorageService = new UserStorageServiceMaster(
                new UserMemoryRepository(),
                Enumerable.Empty<INotificationReceiver>());
            InitUserStoreageService(userStorageService);

            // Act
            userStorageService.Remove(predicate);
        }

        [TestCase(ExpectedException = typeof(NotSupportedException))]
        public void Remove_SlaveService_ExceptionThrown()
        {
            // Arrange
            var userStorageService = new UserStorageServiceSlave(new UserMemoryRepository());

            // Act
            userStorageService.Remove((u) => true);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        public void Search_InvalidPredicate_ExceptionThrown(Predicate<User> predicate)
        {
            // Arrange
            var userStorageService = new UserStorageServiceMaster(
                new UserMemoryRepository(),
                Enumerable.Empty<INotificationReceiver>());
            InitUserStoreageService(userStorageService);

            // Act
            userStorageService.Search(predicate);
        }

        [Test]
        public void Search_NotExistingUser_FindNoUsers()
        {
            // Arrange
            var userStorageService = new UserStorageServiceMaster(
                new UserMemoryRepository(),
                Enumerable.Empty<INotificationReceiver>());
            InitUserStoreageService(userStorageService);
            Predicate<User> predicate = (u) => u.FirstName == "NotExistingName";

            // Act
            var users = userStorageService.Search(predicate);

            // Assert
            Assert.AreEqual(0, users.Count());
        }

        [Test, TestCaseSource(nameof(SearchPredicates))]
        public int Search_ExistingUser_FindSomeUsers(Predicate<User> predicate)
        {
            // Arrange
            var userStorageService = new UserStorageServiceMaster(
                new UserMemoryRepository(),
                Enumerable.Empty<INotificationReceiver>());
            InitUserStoreageService(userStorageService);

            // Act
            var users = userStorageService.Search(predicate);

            // Assert
            return users.Count();
        }
    }
}
