using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UserStorageServices.Exceptions;
using UserStorageServices.Repositories;

namespace UserStorageServices.Tests
{
    using static UserTestHealper;

    public class UserStorageServiceTests
    {
        public IEnumerable<TestCaseData> InvalidUsers
        {
            get
            {
                User user;

                yield return new TestCaseData(null).Throws(typeof(ArgumentNullException));

                user = GetValidUser();
                user.FirstName = null;
                yield return new TestCaseData(user).Throws(typeof(FirstNameIsNullOrEmptyException));

                user = GetValidUser();
                user.FirstName = "  ";
                yield return new TestCaseData(user).Throws(typeof(FirstNameIsNullOrEmptyException));

                user = GetValidUser();
                user.LastName = null;
                yield return new TestCaseData(user).Throws(typeof(LastNameIsNullOrEmptyException));

                user = GetValidUser();
                user.LastName = "   ";
                yield return new TestCaseData(user).Throws(typeof(LastNameIsNullOrEmptyException));

                user = GetValidUser();
                user.Age = -1;
                yield return new TestCaseData(user).Throws(typeof(AgeExceedsLimitsException));
            }
        }

        public IEnumerable<TestCaseData> UsersInStorage
        {
            get
            {
                foreach (var user in Users)
                {
                    yield return new TestCaseData(user);
                }
            }
        }

        public IEnumerable<TestCaseData> SearchPredicates
        {
            get
            {
                Predicate<User> predicate = u => u.FirstName == "FirstName1";
                yield return new TestCaseData(predicate).Returns(2);

                predicate = u => u.FirstName == "FirstName2";
                yield return new TestCaseData(predicate).Returns(1);

                predicate = u => u.Age > 12;
                yield return new TestCaseData(predicate).Returns(2);

                predicate = u => (u.FirstName == "FirstName1") && (u.LastName == "LastName2");
                yield return new TestCaseData(predicate).Returns(1);

                predicate = u => (u.FirstName == "FirstName1") && (u.Age == 10);
                yield return new TestCaseData(predicate).Returns(1);

                predicate = u => (u.LastName == "LastName2") && (u.Age <= 20);
                yield return new TestCaseData(predicate).Returns(2);

                predicate = u => (u.FirstName == "FirstName1") && (u.LastName == "LastName2") && (u.Age == 15);
                yield return new TestCaseData(predicate).Returns(1);
            }
        }

        [Test, TestCaseSource(nameof(InvalidUsers))]
        public void Add_InvalidUser_ExceptionThrown(User user)
        {
            // Arrange
            var userStorageService = new UserStorageServiceMaster(new UserMemoryRepository(),null);

            // Act
            userStorageService.Add(user);
        }

        [Test]
        public void Add_ValidUser_StorageCountIs1()
        {
            // Arrange
            var userStorageService = new UserStorageServiceMaster(new UserMemoryRepository(),null);
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
            var userStorageService = new UserStorageServiceMaster(new UserMemoryRepository(), null);
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
            var userStorageService = new UserStorageServiceMaster(new UserMemoryRepository(), null);
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
            var userStorageService = new UserStorageServiceMaster(new UserMemoryRepository(), null);
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
            var userStorageService = new UserStorageServiceMaster(new UserMemoryRepository(), null);
            InitUserStoreageService(userStorageService);

            // Act
            userStorageService.Search(predicate);
        }

        [Test]
        public void Search_NotExistingUser_FindNoUsers()
        {
            // Arrange
            var userStorageService = new UserStorageServiceMaster(new UserMemoryRepository(), null);
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
            var userStorageService = new UserStorageServiceMaster(new UserMemoryRepository(), null);
            InitUserStoreageService(userStorageService);

            // Act
            var users = userStorageService.Search(predicate);

            // Assert
            return users.Count();
        }
    }
}
