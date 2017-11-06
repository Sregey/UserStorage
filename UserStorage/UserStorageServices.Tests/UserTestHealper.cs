using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UserStorageServices.Repositories;

namespace UserStorageServices.Tests
{
    internal static class UserTestHealper
    {
        static UserTestHealper()
        {
            Users = new List<User>
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
                    Id = new Guid("00000000-0000-0000-0000-000000000002"),
                    FirstName = "FirstName2",
                    LastName = "LastName2",
                    Age = 20,
                },
                new User
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000003"),
                    FirstName = "FirstName1",
                    LastName = "LastName2",
                    Age = 15,
                },
            };
        }

        public static List<User> Users { get; }

        public static void AssertAreEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, Comparison<T> comparison)
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

        public static int CompareUsers(User user1, User user2)
        {
            int result;
            result = user1.Id.CompareTo(user2.Id);
            if (result != 0)
            {
                return result;
            }

            result = user1.FirstName.CompareTo(user2.FirstName);
            if (result != 0)
            {
                return result;
            }

            result = user1.LastName.CompareTo(user2.LastName);
            if (result != 0)
            {
                return result;
            }

            result = user1.Age.CompareTo(user2.Age);
            if (result != 0)
            {
                return result;
            }

            return result;
        }

        public static void InitUserRepository(IUserRepository userRepository)
        {
            foreach (var user in Users)
            {
                userRepository.Set(user);
            }
        }

        public static User GetValidUser()
        {
            return new User
            {
                FirstName = "FirstName1",
                LastName = "LastName1",
                Age = 10,
            };
        }

        public static void InitUserStoreageService(IUserStorageService userStorageService)
        {
            foreach (var user in Users)
            {
                userStorageService.Add(user);
            }
        }
    }
}
