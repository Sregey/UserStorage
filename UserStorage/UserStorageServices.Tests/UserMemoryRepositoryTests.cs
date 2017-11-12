using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UserStorageServices.Repositories;

namespace UserStorageServices.Tests
{
    using static UserTestHealper;

    internal class UserMemoryRepositoryTests
    {
        public static IEnumerable<TestCaseData> InvalidUsers => UserTestHealper.InvalidUsers;

        public IEnumerable<TestCaseData> UsersInRepository => UserTestHealper.UsersInRepository;

        public IEnumerable<TestCaseData> QueryPredicates => UserTestHealper.SearchPredicates;

        [Test, TestCaseSource(nameof(InvalidUsers))]
        public void Set_InvalidUser_ExceptionThrown(User user)
        {
            // Arrange
            var userMemoryCache = new UserMemoryRepository();

            // Act
            userMemoryCache.Set(user);
        }

        [Test]
        public void Set_ValidUser_StorageCountIs1()
        {
            // Arrange
            var userMemoryCache = new UserMemoryRepository();
            var user = GetValidUser();

            // Act
            userMemoryCache.Set(user);

            // Assert
            Assert.AreEqual(1, userMemoryCache.Count);
        }

        [Test, TestCaseSource(nameof(UsersInRepository))]
        public void Delete_ExistingUser_RemoveOneUser(User user)
        {
            // Arrange
            var userMemoryCache = new UserMemoryRepository();
            InitUserRepository(userMemoryCache);
            int oldUserCount = userMemoryCache.Count;

            // Act
            userMemoryCache.Delete((u) => u == user);

            // Assert
            Assert.AreEqual(oldUserCount - 1, userMemoryCache.Count);
        }

        [Test]
        public void Delete_NotExistingUser_RemoveNoUsers()
        {
            // Arrange
            var userMemoryCache = new UserMemoryRepository();
            InitUserRepository(userMemoryCache);
            int oldUserCount = userMemoryCache.Count;
            var user = GetValidUser();

            // Act
            userMemoryCache.Delete((u) => u == user);

            // Assert
            Assert.AreEqual(oldUserCount, userMemoryCache.Count);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        public void Delete_InvalidPredicate_ExceptionThrown(Predicate<User> predicate)
        {
            // Arrange
            var userMemoryCache = new UserMemoryRepository();
            InitUserRepository(userMemoryCache);

            // Act
            userMemoryCache.Delete(predicate);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        public void Query_InvalidPredicate_ExceptionThrown(Predicate<User> predicate)
        {
            // Arrange
            var userMemoryCache = new UserMemoryRepository();
            InitUserRepository(userMemoryCache);

            // Act
            userMemoryCache.Query(predicate);
        }

        [Test]
        public void Query_NotExistingUser_FindNoUsers()
        {
            // Arrange
            var userMemoryCache = new UserMemoryRepository();
            InitUserRepository(userMemoryCache);
            Predicate<User> predicate = (u) => u.FirstName == "NotExistingName";

            // Act
            var users = userMemoryCache.Query(predicate);

            // Assert
            Assert.AreEqual(0, users.Count());
        }

        [Test, TestCaseSource(nameof(QueryPredicates))]
        public int Search_ExistingUser_FindSomeUsers(Predicate<User> predicate)
        {
            // Arrange
            var userMemoryCache = new UserMemoryRepository();
            InitUserRepository(userMemoryCache);

            // Act
            var users = userMemoryCache.Query(predicate);

            // Assert
            return users.Count();
        }
    }
}
