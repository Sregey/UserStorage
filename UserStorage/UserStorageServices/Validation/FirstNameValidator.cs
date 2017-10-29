using UserStorageServices.Exceptions;

namespace UserStorageServices.Validation
{
    internal class FirstNameValidator : IValidator<User>
    {
        public void Validate(User user)
        {
            if (string.IsNullOrWhiteSpace(user.FirstName))
            {
                throw new FirstNameIsNullOrEmptyException();
            }
        }
    }
}
