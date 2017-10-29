using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorageServices
{
    public class UserStorageServiceSlaveLog : UserStorageServiceLog<UserStorageServiceSlave>, INotificationSubscriber
    {
        public UserStorageServiceSlaveLog(UserStorageServiceSlave userStorageService)
            : base(userStorageService)
        {
        }

        public void UserAdded(object sender, UserStorageModifiedEventArgs args)
        {
            UserStorageService.UserAdded(sender, args);
        }

        public void UserRemoved(object sender, UserStorageModifiedEventArgs args)
        {
            UserStorageService.UserRemoved(sender, args);
        }
    }
}
