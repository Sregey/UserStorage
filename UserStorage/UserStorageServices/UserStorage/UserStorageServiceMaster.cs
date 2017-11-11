using System;
using System.Collections.Generic;
using UserStorageServices.Notification;
using UserStorageServices.Repositories;
using UserStorageServices.Validation;

namespace UserStorageServices.UserStorage
{
    public class UserStorageServiceMaster : UserStorageServiceBase
    {
        private readonly IIdGenerator idGenerator;
        private readonly IValidator<User> userValidator;

        private readonly INotificationSender notificationSender;

        public UserStorageServiceMaster(IUserRepository userRepository, IEnumerable<INotificationReceiver> receivers)
            : base(userRepository)
        {
            notificationSender = new CompositeNotificationSender(receivers);

            idGenerator = new IdGenerator(userRepository.LastId);
            userValidator = new UserValidator();
        }

        public override UserStorageServiceMode ServiceMode => UserStorageServiceMode.MasterMode;

        public override void Add(User user)
        {
            userValidator.Validate(user);
            user.Id = idGenerator.Generate();
            UserRepository.Set(user);

            OnUserAdded(user);
        }

        public override void Remove(Predicate<User> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            var removedUser = UserRepository.Delete(predicate);
            if (removedUser != null)
            {
                OnUserRemoved(removedUser.Id);
            }
        }

        protected virtual void OnUserAdded(User user)
        {
            notificationSender.Send(new NotificationContainer()
            {
                Notifications = new[]
                {
                    new Notification.Notification()
                    {
                        Type = NotificationType.AddUser,
                        Action = new AddUserActionNotification() { User = user }
                    }
                }
            });
        }

        protected virtual void OnUserRemoved(Guid userId)
        {
            notificationSender.Send(new NotificationContainer()
            {
                Notifications = new[]
                {
                    new Notification.Notification()
                    {
                        Type = NotificationType.DeleteUser,
                        Action = new DeleteUserActionNotification() { UserId = userId }
                    }
                }
            });
        }
    }
}
