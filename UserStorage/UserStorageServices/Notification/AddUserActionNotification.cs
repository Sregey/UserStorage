using System.Xml.Serialization;

namespace UserStorageServices.Notification
{
    public class AddUserActionNotification
    {
        [XmlElement("user")]
        public User User { get; set; }
    }
}
