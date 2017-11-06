using System;

namespace UserStorageServices.Notification
{
    internal class NotificationSender : INotificationSender
    {
        private readonly INotificationReceiver receiver;

        public NotificationSender(INotificationReceiver receiver)
        {
            this.receiver = receiver;
        }

        public void Send(NotificationContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            receiver.Receive(container);
        }
    }
}
