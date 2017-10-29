using System;

namespace UserStorageServices
{
    internal class UserValidator : IValidator<User>
    {
        private IValidator<User>[] validators;

        public UserValidator()
        {
            validators = new IValidator<User>[]
            {
                new FirstNameValidator(), 
                new LastNameValidator(), 
                new AgeValidator(), 
            };
        }

        public void Validate(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            foreach (var validator in validators)
            {
                validator.Validate(user);
            }
        }
    }
}
