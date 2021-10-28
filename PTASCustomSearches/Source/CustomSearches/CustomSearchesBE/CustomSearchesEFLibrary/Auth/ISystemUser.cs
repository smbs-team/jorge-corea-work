namespace CustomSearchesEFLibrary.Auth
{
    using System;

    /// <summary>
    /// Interface that contains information about a user.
    /// </summary>
    public interface ISystemUser
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public Guid Systemuserid { get; set; }

        /// <summary>
        /// Gets or sets user full name.
        /// </summary>
        public string Fullname { get; set; }
    }
}
