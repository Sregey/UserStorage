using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace UserStorageServices.UserStorage
{
    public class UserStorageServiceLog : UserStorageServiceDecorator
    {
        private  static readonly BooleanSwitch enableLogging = new BooleanSwitch("enableLogging ", "Enable or disable logging.");

        public UserStorageServiceLog(IUserStorageService userStorageService)
            : base(userStorageService)
        {
        }

        public override int Count => UserStorageService.Count;

        public override void Add(User user)
        {
            UserStorageService.Add(user);
            Log("Add() method is called.");
        }

        public override void Remove(Predicate<User> predicate)
        {
            UserStorageService.Remove(predicate);
            Log("Remove() method is called.");
        }

        public override IEnumerable<User> Search(Predicate<User> predicate)
        {
            var users = UserStorageService.Search(predicate);
            Log("Search() method is called.");
            return users;
        }

        private void Log(string message)
        {
            if (enableLogging.Enabled)
            {
                Trace.WriteLine(message);
            }
        }
    }
}
