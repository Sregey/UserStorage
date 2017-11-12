using System;
using System.Collections.Generic;
using UserStorageServices.Validation.Attributes;

namespace UserStorageServices.Validation
{
    internal abstract class UserPropertyValidator : IValidator<User>
    {
        public abstract void Validate(User item);

        protected IEnumerable<ValidationAttribute> GetValidateAttributesForProperty(string propertyName)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            var property = typeof(User).GetProperty(propertyName);

            if (property == null)
            {
                throw new ArgumentException($"Invalid user property name {propertyName}.");
            }

            return (IEnumerable<ValidationAttribute>)property.GetCustomAttributes(typeof(ValidationAttribute), false);
        }
    }
}
