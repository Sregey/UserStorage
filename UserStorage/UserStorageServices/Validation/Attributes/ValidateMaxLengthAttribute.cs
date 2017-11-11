using System;

namespace UserStorageServices.Validation.Attributes
{
    sealed class ValidateMaxLengthAttribute : ValidationAttribute
    {
        public int MaxLength { get; }

        public ValidateMaxLengthAttribute(int maxLength)
        {
            MaxLength = maxLength;
            ErrorMesage = "Length of property is bigger then max length.";
        }

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
