using System.Xml.Serialization;

namespace UserStorageServices.Notification
{
    internal class AddUserActionNotification
    {
        [XmlElement("user")]
        public User User { get; set; }
    }
}
