using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace UserStorageServices.Notification
{
    class XmlNotificationSerializer : INotificationSerializer
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

            //var result = new StringBuilder();

            //var xmlSetting = new XmlWriterSettings();
            //xmlSetting.Encoding = Encoding.Unicode;

            //using (var xmlWriter = XmlWriter.Create(result, xmlSetting))
            //{
            //    formatter.Serialize(xmlWriter, container);
            //    return result.ToString();
            //}
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
