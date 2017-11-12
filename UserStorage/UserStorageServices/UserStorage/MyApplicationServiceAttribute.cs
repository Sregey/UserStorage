using System;

namespace UserStorageServices.UserStorage
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class MyApplicationServiceAttribute : Attribute
    {
        public string ServiceMode { get; }

        public MyApplicationServiceAttribute(string serviceMode)
        {
            ServiceMode = serviceMode;
        }
    }
}
