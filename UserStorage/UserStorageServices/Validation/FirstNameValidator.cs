using System;

namespace UserStorageServices
{
    class FirstNameValidator : IValidator<User>
    {
        public void Validate(User user)
        {
            if (string.IsNullOrWhiteSpace(user.FirstName))
            {
                throw new ArgumentException("FirstName is null or empty or whitespace", nameof(user));
            }
        }
    }
}
