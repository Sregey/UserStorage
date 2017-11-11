using System;

namespace UserStorageServices.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    abstract class ValidationAttribute : Attribute
    {
        protected ValidationAttribute()
        {
            ErrorMesage = "Anexcpectrd error.";
        }

        public string ErrorMesage { get; set; }

        public abstract bool IsValid(object value);
    }
}
