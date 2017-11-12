using System;

namespace UserStorageServices.Validation.Attributes
{
    internal sealed class ValidateMinMaxAttribute : ValidationAttribute
    {
        public ValidateMinMaxAttribute(int min, int max)
        {
            Min = min;
            Max = max;
            ErrorMesage = "Value is not in [min, max] range.";
        }

        public int Min { get; }

        public int Max { get; }

        public override bool IsValid(object value)
        {
            var isValid = false;

            if (value.IsNumber() && value is IComparable comp)
            {
                isValid = (comp.CompareTo(Min) > -1) && (comp.CompareTo(Max) < 1);
            }

            return isValid;
        }
    }
}
