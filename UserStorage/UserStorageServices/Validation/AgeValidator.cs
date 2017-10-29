using UserStorageServices.Exceptions;

namespace UserStorageServices.Validation
{
    class AgeValidator : IValidator<User>
    {
        public void Validate(User user)
        {
            if (user.Age < 0)
            {
                throw new AgeExceedsLimitsException();
            }
        }
    }
}
