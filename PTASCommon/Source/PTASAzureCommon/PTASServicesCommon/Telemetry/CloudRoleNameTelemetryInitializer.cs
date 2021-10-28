namespace PTASServicesCommon.Telemetry
{
    using System;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;

    /// <summary>
    /// Telemetry initializer used to add cloud role name.
    /// </summary>
    /// <seealso cref="Microsoft.ApplicationInsights.Extensibility.ITelemetryInitializer" />
    public class CloudRoleNameTelemetryInitializer : ITelemetryInitializer
    {
        /// <summary>
        /// The role name.
        /// </summary>
        private string roleName;

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudRoleNameTelemetryInitializer"/> class.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        public CloudRoleNameTelemetryInitializer(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentNullException("roleName");
            }

            this.roleName = roleName;
        }

        /// <summary>
        /// Initializes properties of the specified <see cref="T:Microsoft.ApplicationInsights.Channel.ITelemetry" /> object.
        /// </summary>
        /// <param name="telemetry">Telemetry class.</param>
        public void Initialize(ITelemetry telemetry)
        {
            // set custom role name here
            telemetry.Context.Cloud.RoleName = this.roleName;
        }
    }
}
