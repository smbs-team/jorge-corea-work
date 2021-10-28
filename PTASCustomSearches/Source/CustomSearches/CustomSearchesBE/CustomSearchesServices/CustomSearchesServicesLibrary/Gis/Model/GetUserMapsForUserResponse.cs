namespace CustomSearchesServicesLibrary.Gis.Model
{
    using CustomSearchesServicesLibrary.Auth.Model;

    /// <summary>
    /// Model for the response of the GetUserMapsForUser service.
    /// </summary>
    public class GetUserMapsForUserResponse
    {
        /// <summary>
        /// Gets or sets the user map data collection.
        /// </summary>
        public UserMapData[] UserMaps { get; set; }

        /// <summary>
        /// Gets or sets the user details.
        /// </summary>
        public UserInfoData[] UsersDetails { get; set; }
    }
}
