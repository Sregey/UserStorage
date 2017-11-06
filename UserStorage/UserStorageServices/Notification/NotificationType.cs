using System.Xml.Serialization;

namespace UserStorageServices.Notification
{
    [XmlType(IncludeInSchema = false)]
    internal enum NotificationType
    {
        [XmlEnum("addUser")]
        AddUser,

        [XmlEnum("deleteUser")]
        DeleteUser
    }
}
