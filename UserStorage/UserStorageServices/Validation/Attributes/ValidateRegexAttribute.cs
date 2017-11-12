using System.Text.RegularExpressions;

namespace UserStorageServices.Validation.Attributes
{
    internal sealed class ValidateRegexAttribute : ValidationAttribute
    {
        public ValidateRegexAttribute(string pattern)
        {
            Pattern = pattern;
            ErrorMesage = "Property not match pattern";
        }

        public string Pattern { get; }

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
