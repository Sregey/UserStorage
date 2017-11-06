using UserStorageServices;
using UserStorageServices.Repositories;
using UserStorageServices.UserStorage;

namespace UserStorageApp
{
    /// <summary>
    /// Represents a client that uses an instance of the <see cref="UserStorageService"/>.
    /// </summary>
    public class Client
    {
        private readonly IUserStorageService userStorageService;
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        public Client(IUserStorageService userStorageService, IUserRepository userRepository)
        {
            this.userStorageService = userStorageService;
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Runs a sequence of actions on an instance of the <see cref="UserStorageService"/> class.
        /// </summary>
        public void Run()
        {
            userRepository.Start();
            userStorageService.Add(new User
            {
                FirstName = "Alex",
                LastName = "Black",
                Age = 25
            });
            userRepository.Stop();

            userRepository.Start();
            userStorageService.Remove((u) => u.FirstName == "Bill");
            userRepository.Stop();

            userRepository.Start();
            userStorageService.Search((u) => u.FirstName == "Alex");
            userRepository.Stop();
        }
    }
}
