using System.Xml.Serialization;

namespace UserStorageServices.Notification
{
    internal class DeleteUserActionNotification
    {
        [XmlElement("userId")]
        public int UserId { get; set; }
    }
}
