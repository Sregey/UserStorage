using System;
using UserStorageServices.Validation.Attributes;

namespace UserStorageServices
{
    /// <summary>
    /// Represents a user.
    /// </summary>
    [Serializable]
    public class User
    {
        /// <summary>
        /// Gets or sets a user id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets a user first name.
        /// </summary>
        [ValidateNotNullOrWhiteSpace]
        [ValidateMaxLength(20)]
        [ValidateRegex("^[A-Za-z]+$")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets a user last name.
        /// </summary>
        [ValidateNotNullOrWhiteSpace]
        [ValidateMaxLength(25)]
        [ValidateRegex("^[A-Za-z]+$")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets a user age.
        /// </summary>
        [ValidateMinMax(3, 130)]
        public int Age { get; set; }
    }
}
