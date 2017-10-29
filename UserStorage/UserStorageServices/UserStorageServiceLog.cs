using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace UserStorageServices
{
    public abstract class UserStorageServiceLog<T> : UserStorageServiceDecorator<T>
        where T : IUserStorageService
    {
        private static BooleanSwitch enableLogging = new BooleanSwitch("enableLogging ", "Enable or disable logging.");

        protected UserStorageServiceLog(IUserStorageService userStorageService)
            : base(userStorageService)
        {
        }

        public override int Count => UserStorageService.Count;

        public override void Add(User user)
        {
            UserStorageService.Add(user);
            this.Log("Add() method is called.");
        }

        public override void RemoveFirst(Predicate<User> predicate)
        {
            UserStorageService.RemoveFirst(predicate);
            this.Log("RemoveFirst() method is called.");
        }

        public override void RemoveAll(Predicate<User> predicate)
        {
            UserStorageService.RemoveAll(predicate);
            this.Log("RemoveAll() method is called.");
        }

        public override IEnumerable<User> Search(Predicate<User> predicate)
        {
            var users = UserStorageService.Search(predicate);
            this.Log("Search() method is called.");
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
