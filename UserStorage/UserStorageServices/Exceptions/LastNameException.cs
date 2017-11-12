using System;

namespace UserStorageServices.Exceptions
{
    public class LastNameException : Exception
    {
        public LastNameException()
            : base("LastName has invalid format.")
        {
        }

        public LastNameException(string message)
            : base(message)
        {
        }

        public LastNameException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
