using UserStorageServices.Exceptions;

namespace UserStorageServices.Validation
{
    internal class FirstNameValidator : UserPropertyValidator
    {
        public override void Validate(User user)
        {
            foreach (var validationAttribute in GetValidateAttributesForProperty(nameof(user.FirstName)))
            {
                if (!validationAttribute.IsValid(user.FirstName))
                {
                    throw new FirstNameException(validationAttribute.ErrorMesage);
                }
            }
        }
    }
}
