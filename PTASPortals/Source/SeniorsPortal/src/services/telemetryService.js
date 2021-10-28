import { ApplicationInsights } from '@microsoft/applicationinsights-web';
import { ReactPlugin } from '@microsoft/applicationinsights-react-js';
import { createBrowserHistory } from 'history';

let reactPlugin = null;
let appInsights = null;

/**
 * Create the App Insights Telemetry Service
 * @return {{reactPlugin: ReactPlugin, appInsights: Object, initialize: Function}} - Object
 */

const createTelemetryService = () => {
  /**
   * Initialize the Application Insights class
   * @param {string} instrumentationKey - Application Insights Instrumentation Key
   * @param {Object} browserHistory - client's browser history, supplied by the withRouter HOC
   * @return {void}
   */
  const initialize = (instrumentationKey, browserHistory) => {
    if (!browserHistory) {
      browserHistory = createBrowserHistory({ basename: '' });
    }
    if (!instrumentationKey) {
      instrumentationKey = process.env.REACT_APP_TELEMETRY_INSTRUMENTATION_KEY;
    }

    reactPlugin = new ReactPlugin();

    appInsights = new ApplicationInsights({
      config: {
        instrumentationKey: instrumentationKey,
        enableAutoRouteTracking: true,
        maxBatchInterval: 0,
        disableFetchTracking: false,
        extensions: [reactPlugin],
        extensionConfig: {
          [reactPlugin.identifier]: {
            history: browserHistory,
          },
        },
      },
    });

    appInsights.loadAppInsights();
  };

  return { reactPlugin, appInsights, initialize };
  //};
};

export const ai = createTelemetryService();
export const getAppInsights = () => appInsights;
export const getAppInsightsInstance = () => {
  const ai = createTelemetryService();
  ai.initialize();
  const appInsights = getAppInsights();
  return appInsights;
};
