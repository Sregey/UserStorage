using System;

namespace UserStorageServices.Exceptions
{
    public class AgeExceedsLimitsException : Exception
    {
        public AgeExceedsLimitsException()
            : base("Age is negative")
        {
        }

        public AgeExceedsLimitsException(string message)
            : base(message)
        {
        }

        public AgeExceedsLimitsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
