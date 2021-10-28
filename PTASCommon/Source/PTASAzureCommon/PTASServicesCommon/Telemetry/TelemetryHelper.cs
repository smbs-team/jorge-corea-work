namespace PTASServicesCommon.Telemetry
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DependencyCollector;
    using Microsoft.ApplicationInsights.Extensibility;

    /// <summary>
    /// Telemetry helper for Application Insights.
    /// </summary>
    public static class TelemetryHelper
    {
        /// <summary>
        /// The active telemetry channels.
        /// </summary>
        private static string[] activeTelemetryChannels;

        /// <summary>
        /// Creates the telemetry client.
        /// </summary>
        /// <param name="roleName">Name of the role that will be used for the telemetry.</param>
        /// <param name="instrumentationKey">The instrumentation key.</param>
        /// <returns>
        /// The telemetry client.
        /// </returns>
        public static TelemetryClient CreateTelemetryClient(string roleName, string instrumentationKey)
        {
            var telemetryConfiguration = new TelemetryConfiguration();
            telemetryConfiguration.InstrumentationKey = instrumentationKey;
            telemetryConfiguration.TelemetryInitializers.Add(new CloudRoleNameTelemetryInitializer(roleName));
            telemetryConfiguration.TelemetryInitializers.Add(new HttpDependenciesParsingTelemetryInitializer());

            using (DependencyTrackingTelemetryModule depModule = new DependencyTrackingTelemetryModule())
            {
                depModule.Initialize(telemetryConfiguration);
            }

            return new TelemetryClient(telemetryConfiguration);
        }

        /// <summary>
        /// Sets the active telemetry channels.
        /// </summary>
        /// <param name="activeChannels">The active channels.</param>
        public static void SetActiveTelemetryChannels(string[] activeChannels)
        {
            TelemetryHelper.activeTelemetryChannels = activeChannels;
        }

        /// <summary>
        /// Tracks the performance of an action.
        /// </summary>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="actionToTrack">Action that the performance is tracked for.</param>
        /// <param name="telemetryClient">The telemetry client.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="telemetryChannel">The telemetry channel.  If null the telemetry will always be executed.
        /// Otherwise the channel will be checked against the active channels.</param>
        /// <returns></returns>
        public static async Task TrackPerformanceAsync(
            string eventName,
            Func<Task<Dictionary<string, double>>> actionToTrack,
            TelemetryClient telemetryClient,
            Dictionary<string, string> properties = null,
            string telemetryChannel = null)
        {
            try
            {
                if (telemetryClient == null || !TelemetryHelper.IsActiveTelemetryChannel(telemetryChannel))
                {
                    await actionToTrack();
                    return;
                }

                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                var metrics = (await actionToTrack()) ?? new Dictionary<string, double>();
                stopWatch.Stop();
                metrics.Add($"{eventName}:TimeMs", stopWatch.ElapsedMilliseconds);

                telemetryClient.TrackEvent(eventName, properties, metrics);
            }
            catch (Exception)
            {
                // We don't want telemetry errors to disrupt the service.
            }
        }

        /// <summary>
        /// Determines whether the telemmetry channel is active.
        /// </summary>
        /// <param name="telemetryChannel">The telemetry channel.</param>
        /// <returns>
        ///   <c>true</c> if the telemetry channel is actve; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsActiveTelemetryChannel(string telemetryChannel)
        {
            if (string.IsNullOrEmpty(telemetryChannel))
            {
                return true;
            }

            if (TelemetryHelper.activeTelemetryChannels == null)
            {
                return false;
            }

            return
                (from atc in TelemetryHelper.activeTelemetryChannels
                    where atc.Trim().Equals(telemetryChannel.Trim(), StringComparison.OrdinalIgnoreCase)
                select atc).Count() > 0;
        }
    }
}