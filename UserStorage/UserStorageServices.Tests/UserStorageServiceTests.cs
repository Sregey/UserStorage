using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace UserStorageServices.Tests
{
    public class UserStorageServiceTests
    {
        public IEnumerable<TestCaseData> InvalidUsers
        {
            get
            {
                yield return new TestCaseData(null).Throws(typeof(ArgumentNullException));
                yield return new TestCaseData(new User {FirstName = null}).Throws(typeof(ArgumentException));
            }
        }

    [Test,TestCaseSource("InvalidUsers")]
    public static void Add(User user)
        {
            // Arrange
            var userStorageService = new UserStorageService();

            // Act
            userStorageService.Add(user);
        }
    }
}
