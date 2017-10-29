using System;

namespace UserStorageServices
{
    class LastNameValidator : IValidator<User>
    {
        public void Validate(User user)
        {
            if (string.IsNullOrWhiteSpace(user.LastName))
            {
                throw new ArgumentException("LastName is null or empty or whitespace", nameof(user));
            }
        }
    }
}
