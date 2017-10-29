using UserStorageServices.Exceptions;

namespace UserStorageServices.Validation
{
    internal class LastNameValidator : IValidator<User>
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
