/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { ApplicationInsights } from '@microsoft/applicationinsights-web';
import { ReactPlugin } from '@microsoft/applicationinsights-react-js';
import { createBrowserHistory } from 'history';

/* eslint-disable */
/**
 * @remarks
 * See:
 * https://www.npmjs.com/package/@microsoft/applicationinsights-react-js
 * https://docs.microsoft.com/es-es/azure/azure-monitor/app/javascript-react-plugin
 * https://github.com/Azure-Samples/application-insights-react-demo/tree/master/src
 * https://github.com/microsoft/applicationinsights-js
 * https://docs.microsoft.com/en-us/azure/azure-monitor/app/api-filtering-sampling#filtering
 */
/* eslint-enable */

const browserHistory = createBrowserHistory({ basename: '' });
const reactPlugin = new ReactPlugin();
const appInsights = new ApplicationInsights({
  config: {
    instrumentationKey: process.env.REACT_APP_TELEMETRY_INSTRUMENTATION_KEY,
    enableAutoRouteTracking: true,
    maxBatchInterval: 0,
    disableFetchTracking: false,
    extensions: [reactPlugin],
    extensionConfig: {
      [reactPlugin.identifier]: { history: browserHistory },
    },
  },
});
appInsights.loadAppInsights();

export { reactPlugin, appInsights };
