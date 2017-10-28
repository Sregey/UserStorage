using System;
using System.Collections.Generic;
using System.Xml.Schema;
using NUnit.Framework;

namespace UserStorageServices.Tests
{
    public class UserStorageServiceTests
    {
        private void MakeValidUser(User user)
        {
            user.Id = new Guid();
            user.FirstName = "FirstName";
            user.LastName = "LastName";
            user.Age = 10;

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
    }
}
