using System.Text.RegularExpressions;

namespace UserStorageServices.Validation.Attributes
{
    sealed class ValidateRegexAttribute : ValidationAttribute
    {
        public string Pattern { get; }

        public ValidateRegexAttribute(string pattern)
        {
            Pattern = pattern;
            ErrorMesage = "Property not match pattern";
        }

        public override bool IsValid(object value)
        {
            var isValid = false;

            if (value is string s)
            {
                var regex = new Regex(Pattern);
                isValid = regex.IsMatch(s);
            }

            return isValid;
        }
    }
}
