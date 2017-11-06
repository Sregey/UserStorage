using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UserStorageServices.Repositories;

namespace UserStorageServices.Tests
{
    class UserDiskRepository
    {
        private const string RepositoryFileName = "test_repository.bin";

        private static readonly User[] users;

        static UserDiskRepository()
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

        [TearDown]
        public void DeleteRepositoryFile()
        {
            if (File.Exists(RepositoryFileName))
            {
                File.Delete(RepositoryFileName);
            }
        }

        [Test]
        public void StartAfterStop_ResultBeforStopIsEqualToResultAfterStart()
        {
            // Arrange
            var userMemoryCacheWithStateStop = new Repositories.UserDiskRepository(RepositoryFileName);
            InitUserMemoryCache(userMemoryCacheWithStateStop);

            var userMemoryCacheWithStateStart = new Repositories.UserDiskRepository(RepositoryFileName);

            // Act
            userMemoryCacheWithStateStop.Stop();
            userMemoryCacheWithStateStart.Start();

            // Assert
            AssertAreEqual(
                userMemoryCacheWithStateStop.Query((u) => true),
                userMemoryCacheWithStateStart.Query((u) => true),
                CompareUsers);
        }

        private void InitUserMemoryCache(Repositories.UserDiskRepository userMemoryCache)
        {
            foreach (var user in users)
            {
                userMemoryCache.Set(user);
            }
        }

        private void AssertAreEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, Comparison<T> comparison)
        {
            var expectedUsers = expected.ToArray();
            var actualUsers = actual.ToArray();

            if (expectedUsers.Length != actualUsers.Length)
            {
                throw new AssertionException("Counts of expected users and actual users are different.");
            }

            for (int i = 0; i < expectedUsers.Length; i++)
            {
                if (comparison(expectedUsers[i], actualUsers[i]) != 0)
                {
                    throw new AssertionException("Expected and actual users are not equal.");
                }
            }
        }

        private int CompareUsers(User user1, User user2)
        {
            int result;
            result = user1.Id.CompareTo(user2.Id);
            if (result != 0)
                return result;

            result = user1.FirstName.CompareTo(user2.FirstName);
            if (result != 0)
                return result;

            result = user1.LastName.CompareTo(user2.LastName);
            if (result != 0)
                return result;

            result = user1.Age.CompareTo(user2.Age);
            if (result != 0)
                return result;

            return result;
        }
    }
}
