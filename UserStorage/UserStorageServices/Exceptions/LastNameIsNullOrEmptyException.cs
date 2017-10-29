using System;

namespace UserStorageServices.Exceptions
{
    public class LastNameIsNullOrEmptyException : Exception
    {
        public LastNameIsNullOrEmptyException()
            : base("LastName is null or empty or whitespace")
        {
        }

        public LastNameIsNullOrEmptyException(string message)
            : base(message)
        {
        }

        public LastNameIsNullOrEmptyException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
