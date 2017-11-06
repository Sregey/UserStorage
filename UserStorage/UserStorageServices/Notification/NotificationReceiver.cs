using System;

namespace UserStorageServices.Notification
{
    public class NotificationReceiver : INotificationReceiver
    {
        public event Action<NotificationContainer> Received = delegate { }; 

        public void Receive(NotificationContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            OnReceived(container);
        }

        protected virtual void OnReceived(NotificationContainer container)
        {
            Received(container);
        }
    }
}
