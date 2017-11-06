using System.Xml.Serialization;

namespace UserStorageServices.Notification
{
    [XmlRoot("NotificationContainer", IsNullable = false, Namespace = "http://tempuri.org/userService/notification")]
    internal class NotificationContainer
    {
        [XmlArray("notifications")]
        [XmlArrayItem("notification")]
        public Notification[] Notifications { get; set; }
    }
}
