using System;
using UserStorageServices;
using UserStorageServices.Repositories;
using UserStorageServices.UserStorage;

namespace UserStorageApp
{
    /// <summary>
    /// Represents a client that uses an instance of the <see cref="UserStorageService"/>.
    /// </summary>
    public class Client : MarshalByRefObject
    {
        private readonly IUserStorageService userStorageService;
        private readonly IUserRepositoryManager userRepositoryManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        public Client(IUserStorageService userStorageService, IUserRepositoryManager userRepositoryManager)
        {
            this.userStorageService = userStorageService;
            this.userRepositoryManager = userRepositoryManager;
        }

        /// <summary>
        /// Runs a sequence of actions on an instance of the <see cref="UserStorageService"/> class.
        /// </summary>
        public void Run()
        {
            userRepositoryManager.Start();
            userStorageService.Add(new User
            {
                FirstName = "Alex",
                LastName = "Black",
                Age = 25
            });
            userRepositoryManager.Stop();

            userRepositoryManager.Start();
            userStorageService.Remove((u) => u.FirstName == "Bill");
            userRepositoryManager.Stop();

            userRepositoryManager.Start();
            userStorageService.Search((u) => u.FirstName == "Alex");
            userRepositoryManager.Stop();
        }
    }
}
