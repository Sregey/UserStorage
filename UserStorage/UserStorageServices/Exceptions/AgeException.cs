using System;

namespace UserStorageServices.Exceptions
{
    public class AgeException : Exception
    {
        public AgeException()
            : base("Age has invalid format.")
        {
        }

        public AgeException(string message)
            : base(message)
        {
        }

        public AgeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
