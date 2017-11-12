using System;

namespace UserStorageServices.Validation.Attributes
{
    internal sealed class ValidateMaxLengthAttribute : ValidationAttribute
    {
        public ValidateMaxLengthAttribute(int maxLength)
        {
            MaxLength = maxLength;
            ErrorMesage = "Length of property is bigger then max length.";
        }

        public int MaxLength { get; }

        public override bool IsValid(object value)
        {
            var isValid = false;

            if (value is string s)
            {
                isValid = s.Length <= MaxLength;
            }
            else if (value is Array array)
            {
                isValid = array.Length <= MaxLength;
            }

            return isValid;
        }
    }
}
