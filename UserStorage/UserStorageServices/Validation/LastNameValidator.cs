using UserStorageServices.Exceptions;

namespace UserStorageServices.Validation
{
    class LastNameValidator : IValidator<User>
    {
        public void Validate(User user)
        {
            if (string.IsNullOrWhiteSpace(user.LastName))
            {
                throw new LastNameIsNullOrEmptyException();
            }
        }
    }
}
