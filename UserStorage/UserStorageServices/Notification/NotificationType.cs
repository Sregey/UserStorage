using System.Xml.Serialization;

namespace UserStorageServices.Notification
{
    [XmlType(IncludeInSchema = false)]
    public enum NotificationType
    {
        [XmlEnum("addUser")]
        AddUser,

        [XmlEnum("deleteUser")]
        DeleteUser
    }
}
