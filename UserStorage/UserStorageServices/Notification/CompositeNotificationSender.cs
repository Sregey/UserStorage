using System;
using System.Collections.Generic;
using System.Linq;

namespace UserStorageServices.Notification
{
    internal class CompositeNotificationSender : INotificationSender
    {
        private readonly INotificationReceiver[] receivers;
        private readonly INotificationSerializer serializer;

        public CompositeNotificationSender(IEnumerable<INotificationReceiver> receivers)
        {
            this.receivers = receivers.ToArray();
            serializer = new XmlNotificationSerializer();
        }

        public void Send(NotificationContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            foreach (var receiver in receivers)
            {
                receiver.Receive(serializer.Serialize(container));
            }
        }
    }
}
