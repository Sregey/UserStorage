using System;
using System.IO;
using System.Xml.Serialization;

namespace UserStorageServices.Notification
{
    internal class XmlNotificationSerializer : INotificationSerializer
    {
        public string Serialize(NotificationContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            XmlSerializer formatter = new XmlSerializer(typeof(NotificationContainer));

            using (var stringWriter = new StringWriter())
            {
                formatter.Serialize(stringWriter, container);
                return stringWriter.ToString();
            }
        }

        public NotificationContainer Deserialize(string container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            XmlSerializer formatter = new XmlSerializer(typeof(NotificationContainer));

            using (var stringReader = new StringReader(container))
            {
                return (NotificationContainer)formatter.Deserialize(stringReader);
            }
        }
    }
}
