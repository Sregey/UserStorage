using System;

namespace UserStorageServices.Notification
{
    public class NotificationReceiver : MarshalByRefObject, INotificationReceiver
    {
        private readonly INotificationSerializer serializer;

        public NotificationReceiver()
        {
            serializer = new XmlNotificationSerializer();
        }

        public event Action<NotificationContainer> Received = delegate { }; 

        public void Receive(string serializedNotification)
        {
            if (serializedNotification == null)
            {
                throw new ArgumentNullException(nameof(serializedNotification));
            }

            OnReceived(serializedNotification);
        }

        protected virtual void OnReceived(string serializedNotification)
        {
            var container = serializer.Deserialize(serializedNotification);
            Received(container);
        }
    }
}
