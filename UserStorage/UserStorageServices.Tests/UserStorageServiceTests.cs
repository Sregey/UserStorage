using System;
using System.Collections.Generic;
using System.Xml.Schema;
using NUnit.Framework;

namespace UserStorageServices.Tests
{
    public class UserStorageServiceTests
    {
        private UserStorageService userStorageService;
        private static User[] users;

        static UserStorageServiceTests()
        {
            users = new User[]
            {
                new User
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000001"),
                    FirstName = "FirstName1",
                    LastName = "LastName1",
                    Age = 10,
                },
                new User
                {
                    Id =  new Guid("00000000-0000-0000-0000-000000000002"),
                    FirstName = "FirstName2",
                    LastName = "LastName2",
                    Age = 20,
                },
                new User
                {
                    Id =  new Guid("00000000-0000-0000-0000-000000000003"),
                    FirstName = "FirstName1",
                    LastName = "LastName2",
                    Age = 15,
                },
            };
        }

        private void MakeValidUser(User user)
        {
            user.Id = new Guid("00000000-0000-0000-0000-000000000001");
            user.FirstName = "FirstName1";
            user.LastName = "LastName1";
            user.Age = 10;

        }

        [SetUp]
        public void InitUserStoreageService()
        {
            userStorageService = new UserStorageService();

            foreach (var user in users)
            {
                userStorageService.Add(user);
            }
        }

        public IEnumerable<TestCaseData> InvalidUsers
        {
            get
            {
                User user = new User();

                yield return new TestCaseData(null).Throws(typeof(ArgumentNullException));

                MakeValidUser(user);
                user.FirstName = null;
                yield return new TestCaseData(user).Throws(typeof(ArgumentException));

                MakeValidUser(user);
                user.FirstName = "  ";
                yield return new TestCaseData(user).Throws(typeof(ArgumentException));


                MakeValidUser(user);
                user.LastName = null;
                yield return new TestCaseData(user).Throws(typeof(ArgumentException));

                MakeValidUser(user);
                user.LastName = "   ";
                yield return new TestCaseData(user).Throws(typeof(ArgumentException));

                MakeValidUser(user);
                user.Age = -1;
                yield return new TestCaseData(user).Throws(typeof(ArgumentException));
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

        public IEnumerable<TestCaseData> RemoveAll_Predicates
        {
            get
            {
                Predicate<User> predicate = ((u) => u.FirstName == "FirstName1");
                yield return new TestCaseData(predicate).Returns(2);

                predicate = ((u) => u.FirstName == "FirstName2");
                yield return new TestCaseData(predicate).Returns(1);

                predicate = ((u) => u.Age > 12);
                yield return new TestCaseData(predicate).Returns(2);

                //foreach (var user in users)
                //{
                //    Predicate<User> predicate = (u) => u.FirstName == user.FirstName;
                //    yield return new TestCaseData(predicate);
                //}
            }
        }

        [Test, TestCaseSource("InvalidUsers")]
        public void Add_InvalidUser_ExceptionThrown(User user)
        {
            // Arrange
            var userStorageService = new UserStorageService();

            // Act
            userStorageService.Add(user);
        }

        [Test]
        public void Add_ValidUser_StorageCountIs1()
        {
            // Arrange
            var userStorageService = new UserStorageService();
            var user = new User();
            MakeValidUser(user);

            // Act
            userStorageService.Add(user);

            //Assert
            Assert.AreEqual(1, userStorageService.Count);
        }

        [Test]
        public void Add_ExistingUser_ExceptionThrown()
        {
            // Arrange
            var user = new User();
            MakeValidUser(user);

            var userStorageService = new UserStorageService();
            userStorageService.Add(user);

            //Assert
            Assert.Throws<ArgumentException>(() => userStorageService.Add(user));
        }

        [Test, TestCaseSource("UsersInStorage")]
        public void RemoveFirst_ExistingUser_RemoveOneUser(User user)
        {
            // Arrange
            int oldUserCount = userStorageService.Count;

            //Act
            userStorageService.RemoveFirst((u) => u == user);

            //Assert
            Assert.AreEqual(oldUserCount - 1, userStorageService.Count);
        }

        [Test]
        public void RemoveFirst_NotExistingUser_RemoveNoUsers()
        {
            // Arrange
            int oldUserCount = userStorageService.Count;
            var user = new User();
            MakeValidUser(user);

            //Act
            userStorageService.RemoveFirst((u) => u == user);

            //Assert
            Assert.AreEqual(oldUserCount, userStorageService.Count);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        public void RemoveFirst_InvalidPredicate_ExceptionThrown(Predicate<User> predicate)
        {
            //Act
            userStorageService.RemoveFirst(predicate);
        }

        [Test, TestCaseSource("RemoveAll_Predicates")]
        public int RemoveAll_ExistingUser_RemoveSeveralUsers(Predicate<User> predicate)
        {
            // Arrange
            int oldUserCount = userStorageService.Count;

            //Act
            userStorageService.RemoveAll(predicate);

            //Assert
            return oldUserCount - userStorageService.Count;
        }

        [Test]
        public void RemoveAll_NotExistingUser_RemoveNoUsers()
        {
            // Arrange
            int oldUserCount = userStorageService.Count;

            //Act
            userStorageService.RemoveAll((u) => u.FirstName == "NotExistingName");

            //Assert
            Assert.AreEqual(oldUserCount, userStorageService.Count);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        public void RemoveAll_InvalidPredicate_ExceptionThrown(Predicate<User> predicate)
        {
            //Act
            userStorageService.RemoveAll(predicate);
        }

    }
}
