using UserStorageServices.Exceptions;

namespace UserStorageServices.Validation
{
    internal class LastNameValidator : UserPropertyValidator
    {
        public override void Validate(User user)
        {
            foreach (var validationAttribute in GetValidateAttributesForProperty(nameof(user.LastName)))
            {
                if (!validationAttribute.IsValid(user.LastName))
                {
                    throw new LastNameException(validationAttribute.ErrorMesage);
                }
            }
        }
    }
}
