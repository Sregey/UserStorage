using System;

namespace UserStorageServices.Exceptions
{
    public class FirstNameException : Exception
    {
        public FirstNameException()
            : base("FirstName has invalid format.")
        {
        }

        public FirstNameException(string message)
            : base(message)
        {
        }

        public FirstNameException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
