namespace UserStorageServices.Validation.Attributes
{
    internal sealed class ValidateNotNullOrWhiteSpaceAttribute : ValidationAttribute
    {
        public ValidateNotNullOrWhiteSpaceAttribute()
        {
            ErrorMesage = "Property is null or white space.";
        }

        public override bool IsValid(object value)
        {
            var isValid = false;

            if (value is string s)
            {
                isValid = !string.IsNullOrWhiteSpace(s);
            }

            return isValid;
        }
    }
}
