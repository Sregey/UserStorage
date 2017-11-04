using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UserStorageServices.Exceptions;
using UserStorageServices.Repositories;

namespace UserStorageServices.Tests
{
    public class UserStorageServiceTests
    {
        private static User[] users;

        static UserStorageServiceTests()
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

        public IEnumerable<TestCaseData> UsersInStorage
        {
            get
            {
                foreach (var user in users)
                {
                    yield return new TestCaseData(user);
                }
            }
        }

        public IEnumerable<TestCaseData> Search_Predicates
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

        [Test, TestCaseSource("InvalidUsers")]
        public void Add_InvalidUser_ExceptionThrown(User user)
        {
            // Arrange
            var userStorageService = new UserStorageServiceMaster(new UserMemoryCache(),null);

            // Act
            userStorageService.Add(user);
        }

        [Test]
        public void Add_ValidUser_StorageCountIs1()
        {
            // Arrange
            var userStorageService = new UserStorageServiceMaster(new UserMemoryCache(),null);
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
            var userStorageService = new UserStorageServiceSlave(new UserMemoryCache());
            var user = GetValidUser();

            // Assert
            Assert.Throws<NotSupportedException>(() => userStorageService.Add(user));
        }

        [Test, TestCaseSource("UsersInStorage")]
        public void Remove_ExistingUser_RemoveOneUser(User user)
        {
            // Arrange
            var userStorageService = new UserStorageServiceMaster(new UserMemoryCache(), null);
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
            var userStorageService = new UserStorageServiceMaster(new UserMemoryCache(), null);
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
            var userStorageService = new UserStorageServiceMaster(new UserMemoryCache(), null);
            InitUserStoreageService(userStorageService);

            // Act
            userStorageService.Remove(predicate);
        }

        [TestCase(ExpectedException = typeof(NotSupportedException))]
        public void Remove_SlaveService_ExceptionThrown()
        {
            // Arrange
            var userStorageService = new UserStorageServiceSlave(new UserMemoryCache());

            // Act
            userStorageService.Remove((u) => true);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        public void Search_InvalidPredicate_ExceptionThrown(Predicate<User> predicate)
        {
            // Arrange
            var userStorageService = new UserStorageServiceMaster(new UserMemoryCache(), null);
            InitUserStoreageService(userStorageService);

            // Act
            userStorageService.Search(predicate);
        }

        [Test]
        public void Search_NotExistingUser_FindNoUsers()
        {
            // Arrange
            var userStorageService = new UserStorageServiceMaster(new UserMemoryCache(), null);
            InitUserStoreageService(userStorageService);
            Predicate<User> predicate = (u) => u.FirstName == "NotExistingName";

            // Act
            var users = userStorageService.Search(predicate);

            // Assert
            Assert.AreEqual(0, users.Count());
        }

        [Test, TestCaseSource("Search_Predicates")]
        public int Search_ExistingUser_FindSomeUsers(Predicate<User> predicate)
        {
            // Arrange
            var userStorageService = new UserStorageServiceMaster(new UserMemoryCache(), null);
            InitUserStoreageService(userStorageService);

            // Act
            var users = userStorageService.Search(predicate);

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

        private void InitUserStoreageService(IUserStorageService userStorageService)
        {
            foreach (var user in users)
            {
                userStorageService.Add(user);
            }
        }
    }
}
