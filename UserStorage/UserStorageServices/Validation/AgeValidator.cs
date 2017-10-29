using System;

namespace UserStorageServices
{
    class AgeValidator : IValidator<User>
    {
        public void Validate(User user)
        {
            if (user.Age < 0)
            {
                throw new ArgumentException("Age is negative", nameof(user));
            }
        }
    }
}
