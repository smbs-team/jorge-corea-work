namespace CustomSearchesServicesLibrary.Auth
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CustomSearchesEFLibrary.Auth;
    using CustomSearchesServicesLibrary.Auth.Model;

    /// <summary>
    /// User details helper.
    /// </summary>
    public class UserDetailsHelper
    {
        /// <summary>
        /// Gathers the user details.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="userDetails">The user details.</param>
        public static void GatherUserDetails(ISystemUser user, Dictionary<Guid, UserInfoData> userDetails)
        {
            if ((user != null) && (userDetails != null) && (!userDetails.ContainsKey(user.Systemuserid)))
            {
                userDetails.Add(user.Systemuserid, new UserInfoData { Id = user.Systemuserid, FullName = user.Fullname });
            }
        }

        /// <summary>
        /// Gets the user details array from dictionary.
        /// </summary>
        /// <param name="userDetails">The new state.</param>
        /// /// <returns>The user details array.</returns>
        public static UserInfoData[] GetUserDetailsArray(Dictionary<Guid, UserInfoData> userDetails)
        {
            return userDetails.Values.ToArray();
        }
    }
}
