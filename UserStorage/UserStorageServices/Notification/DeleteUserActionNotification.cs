using System;
using System.Xml.Serialization;

namespace UserStorageServices.Notification
{
    internal class DeleteUserActionNotification
    {
        [XmlElement("userId")]
        public Guid UserId { get; set; }
    }
}
