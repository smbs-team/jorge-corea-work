namespace DeploymentHealthService
{
  /// <summary>
  /// System wide constants.
  /// </summary>
  public static class Constants
  {
    /// <summary>
    /// Constant settings.
    /// </summary>
    public static class Settings
    {
      /// <summary>
      /// Gets allowed origins.
      /// </summary>
      public static string AllowedOrigins => "allowed-origins";

      /// <summary>
      /// Auth class.
      /// </summary>
      public static class Auth
      {
        /// <summary>
        /// Gets issuer info.
        /// </summary>
        public static string Issuer => "oauth2:issuer";

        /// <summary>
        /// Gets audience info.
        /// </summary>
        public static string Audience => "oauth2:audience";

        /// <summary>
        /// Gets cert thumbprint.
        /// </summary>
        public static string CertThumbprint => "oauth2:cert-thumbprint";
      }
    }
  }
}