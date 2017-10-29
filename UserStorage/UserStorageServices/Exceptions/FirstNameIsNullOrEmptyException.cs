using System;

namespace UserStorageServices.Exceptions
{
    public class FirstNameIsNullOrEmptyException : Exception
    {
        public FirstNameIsNullOrEmptyException()
            : base("FirstName is null or empty or whitespace")
        {
        }

        public FirstNameIsNullOrEmptyException(string message)
            : base(message)
        {
        }

        public FirstNameIsNullOrEmptyException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
