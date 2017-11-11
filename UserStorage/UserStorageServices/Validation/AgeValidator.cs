using UserStorageServices.Exceptions;

namespace UserStorageServices.Validation
{
    internal class AgeValidator : UserPropertyValidator
    {
        public override void Validate(User user)
        {
            foreach (var validationAttribute in GetValidateAttributesForProperty(nameof(user.Age)))
            {
                if (!validationAttribute.IsValid(user.Age))
                {
                    throw new AgeException(validationAttribute.ErrorMesage);
                }
            }
        }
    }
}
