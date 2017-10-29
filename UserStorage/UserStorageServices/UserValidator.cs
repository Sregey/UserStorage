using System;

namespace UserStorageServices
{
    internal class UserValidator : IUserValidator
    {
        public void Validate(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrWhiteSpace(user.FirstName))
            {
                throw new ArgumentException("FirstName is null or empty or whitespace", nameof(user));
            }

            if (string.IsNullOrWhiteSpace(user.LastName))
            {
                throw new ArgumentException("LastName is null or empty or whitespace", nameof(user));
            }

            if (user.Age < 0)
            {
                throw new ArgumentException("Age is negative", nameof(user));
            }
        }
    }
}
