using System;
using System.Xml.Serialization;

namespace UserStorageServices.Notification
{
    public class DeleteUserActionNotification
    {
        [XmlElement("userId")]
        public Guid UserId { get; set; }
    }
}
