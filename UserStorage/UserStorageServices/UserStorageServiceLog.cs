using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace UserStorageServices
{
    class UserStorageServiceLog : UserStorageServiceDecorator
    {
        private static BooleanSwitch enableLogging = new BooleanSwitch("enableLogging ", "Enable or disable logging.");

        public override int Count => userStorageService.Count;

        public UserStorageServiceLog(IUserStorageService userStorageService)
            : base(userStorageService)
        {
        }

        public override void Add(User user)
        {
            userStorageService.Add(user);
            Log("Add() method is called.");
        }

        public override void RemoveFirst(Predicate<User> predicate)
        {
            userStorageService.RemoveFirst(predicate);
            Log("RemoveFirst() method is called.");
        }

        public override void RemoveAll(Predicate<User> predicate)
        {
            userStorageService.RemoveAll(predicate);
            Log("RemoveAll() method is called.");
        }

        public override IEnumerable<User> Search(Predicate<User> predicate)
        {
            var users = userStorageService.Search(predicate);
            Log("Search() method is called.");
            return users;
        }

        private void Log(string message)
        {
            if (enableLogging.Enabled)
            {
                Console.WriteLine(message);
            }
        }
    }
}
