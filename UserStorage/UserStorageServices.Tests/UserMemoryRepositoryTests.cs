using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UserStorageServices.Exceptions;
using UserStorageServices.Repositories;

namespace UserStorageServices.Tests
{
    class UserMemoryRepositoryTests
    {
        private static User[] users;

        static UserMemoryRepositoryTests()
        {
            users = new User[]
            {
                new User
                {
                    FirstName = "FirstName1",
                    LastName = "LastName1",
                    Age = 10,
                },
                new User
                {
                    FirstName = "FirstName2",
                    LastName = "LastName2",
                    Age = 20,
                },
                new User
                {
                    FirstName = "FirstName1",
                    LastName = "LastName2",
                    Age = 15,
                },
            };
        }

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

        public IEnumerable<TestCaseData> UsersInRepository
        {
            get
            {
                foreach (var user in users)
                {
                    yield return new TestCaseData(user);
                }
            }
        }

        public IEnumerable<TestCaseData> Query_Predicates
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
            var userStorageService = new UserStorageServiceMaster(new UserMemoryRepository(), null);

            // Act
            userStorageService.Add(user);
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
            InitUserMemoryCache(userMemoryCache);
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
            InitUserMemoryCache(userMemoryCache);
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
            InitUserMemoryCache(userMemoryCache);

            // Act
            userMemoryCache.Delete(predicate);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        public void Query_InvalidPredicate_ExceptionThrown(Predicate<User> predicate)
        {
            // Arrange
            var userMemoryCache = new UserMemoryRepository();
            InitUserMemoryCache(userMemoryCache);

            // Act
            userMemoryCache.Query(predicate);
        }

        [Test]
        public void Query_NotExistingUser_FindNoUsers()
        {
            // Arrange
            var userMemoryCache = new UserMemoryRepository();
            InitUserMemoryCache(userMemoryCache);
            Predicate<User> predicate = (u) => u.FirstName == "NotExistingName";

            // Act
            var users = userMemoryCache.Query(predicate);

            // Assert
            Assert.AreEqual(0, users.Count());
        }

        [Test, TestCaseSource(nameof(Query_Predicates))]
        public int Search_ExistingUser_FindSomeUsers(Predicate<User> predicate)
        {
            // Arrange
            var userMemoryCache = new UserMemoryRepository();
            InitUserMemoryCache(userMemoryCache);

            // Act
            var users = userMemoryCache.Query(predicate);

            // Assert
            return users.Count();
        }

        private User GetValidUser()
        {
            return new User
            {
                FirstName = "FirstName1",
                LastName = "LastName1",
                Age = 10,
            };
        }

        private void InitUserMemoryCache(UserMemoryRepository userMemoryCache)
        {
            foreach (var user in users)
            {
                userMemoryCache.Set(user);
            }
        }

    }
}
