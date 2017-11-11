using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UserStorageServices.Exceptions;
using UserStorageServices.Repositories;
using UserStorageServices.UserStorage;

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
                    FirstName = "Alex",
                    LastName = "Black",
                    Age = 10,
                },
                new User
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000002"),
                    FirstName = "Jack",
                    LastName = "Stone",
                    Age = 20,
                },
                new User
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000003"),
                    FirstName = "Alex",
                    LastName = "Stone",
                    Age = 15,
                },
            };
        }

        public static List<User> Users { get; }

        public static IEnumerable<TestCaseData> InvalidUsers
        {
            get
            {
                User user;

                yield return new TestCaseData(null).Throws(typeof(ArgumentNullException));

                user = GetValidUser();
                user.FirstName = null;
                yield return new TestCaseData(user).Throws(typeof(FirstNameException));

                user = GetValidUser();
                user.FirstName = "  ";
                yield return new TestCaseData(user).Throws(typeof(FirstNameException));

                user = GetValidUser();
                user.FirstName = new String('x', 21);
                yield return new TestCaseData(user).Throws(typeof(FirstNameException));

                user = GetValidUser();
                user.FirstName = "Alex22";
                yield return new TestCaseData(user).Throws(typeof(FirstNameException));

                user = GetValidUser();
                user.LastName = null;
                yield return new TestCaseData(user).Throws(typeof(LastNameException));

                user = GetValidUser();
                user.LastName = "   ";
                yield return new TestCaseData(user).Throws(typeof(LastNameException));

                user = GetValidUser();
                user.LastName = new String('x', 26);
                yield return new TestCaseData(user).Throws(typeof(LastNameException));

                user = GetValidUser();
                user.LastName = "Black22";
                yield return new TestCaseData(user).Throws(typeof(LastNameException));

                user = GetValidUser();
                user.Age = -1;
                yield return new TestCaseData(user).Throws(typeof(AgeException));

                user = GetValidUser();
                user.Age = 2;
                yield return new TestCaseData(user).Throws(typeof(AgeException));

                user = GetValidUser();
                user.Age = 131;
                yield return new TestCaseData(user).Throws(typeof(AgeException));
            }
        }

        public static IEnumerable<TestCaseData> SearchPredicates
        {
            get
            {
                Predicate<User> predicate = u => u.FirstName == "Alex";
                yield return new TestCaseData(predicate).Returns(2);

                predicate = u => u.FirstName == "Jack";
                yield return new TestCaseData(predicate).Returns(1);

                predicate = u => u.Age > 12;
                yield return new TestCaseData(predicate).Returns(2);

                predicate = u => (u.FirstName == "Alex") && (u.LastName == "Stone");
                yield return new TestCaseData(predicate).Returns(1);

                predicate = u => (u.FirstName == "Alex") && (u.Age == 10);
                yield return new TestCaseData(predicate).Returns(1);

                predicate = u => (u.LastName == "Stone") && (u.Age <= 20);
                yield return new TestCaseData(predicate).Returns(2);

                predicate = u => (u.FirstName == "Alex") && (u.LastName == "Stone") && (u.Age == 15);
                yield return new TestCaseData(predicate).Returns(1);
            }
        }

        public static IEnumerable<TestCaseData> UsersInRepository
        {
            get
            {
                foreach (var user in Users)
                {
                    yield return new TestCaseData(user);
                }
            }
        }

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
                FirstName = "FirstName",
                LastName = "LastName",
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
