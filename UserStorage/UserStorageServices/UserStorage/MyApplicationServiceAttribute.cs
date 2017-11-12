using System;

namespace UserStorageServices.UserStorage
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class MyApplicationServiceAttribute : Attribute
    {
        public MyApplicationServiceAttribute(string serviceMode)
        {
            ServiceMode = serviceMode;
        }

        public string ServiceMode { get; }
    }
}
