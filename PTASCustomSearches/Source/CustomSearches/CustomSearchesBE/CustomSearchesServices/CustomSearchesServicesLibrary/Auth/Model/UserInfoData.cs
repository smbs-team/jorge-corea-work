namespace CustomSearchesServicesLibrary.Auth.Model
{
    using System;

    /// <summary>
    /// Model for the user info.
    /// </summary>
    public class UserInfoData
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the user full name.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user roles.
        /// </summary>
        public RoleData[] Roles { get; set; }

        /// <summary>
        /// Gets or sets the user roles.
        /// </summary>
        public TeamData[] Teams { get; set; }
    }
}
