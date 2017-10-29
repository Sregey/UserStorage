using System;

namespace UserStorageServices
{
    public class UserStorageModifiedEventArgs : EventArgs
    {
        private readonly User user;

        public UserStorageModifiedEventArgs(User user)
        {
            this.user = user;
        }

        public User User => this.user;
    }
}
