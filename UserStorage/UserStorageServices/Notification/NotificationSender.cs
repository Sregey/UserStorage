using System;

namespace UserStorageServices.Notification
{
    internal class NotificationSender : INotificationSender
    {
        private readonly INotificationReceiver receiver;
        private readonly INotificationSerializer serializer;

        public NotificationSender(INotificationReceiver receiver)
        {
            this.receiver = receiver;
            serializer = new XmlNotificationSerializer();
        }

        public void Send(NotificationContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            receiver.Receive(serializer.Serialize(container));
        }
    }
}
