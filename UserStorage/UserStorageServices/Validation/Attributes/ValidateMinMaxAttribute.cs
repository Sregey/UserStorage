using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorageServices.Validation.Attributes
{
    sealed class ValidateMinMaxAttribute : ValidationAttribute
    {
        public int Min { get; }

        public int Max { get; }

        public ValidateMinMaxAttribute(int min, int max)
        {
            Min = min;
            Max = max;
            ErrorMesage = "Value is not in [min, max] range.";
        }

        public override bool IsValid(object value)
        {
            var isValid = false;

            if ((value.IsNumber()) && (value is IComparable comp))
            {
                isValid = (comp.CompareTo(Min) > -1) && (comp.CompareTo(Max) < 1);
            }

            return isValid;
        }
    }
}
