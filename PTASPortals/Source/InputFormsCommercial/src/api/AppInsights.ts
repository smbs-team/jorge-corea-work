// AppInsights.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { ReactPlugin } from "@microsoft/applicationinsights-react-js";
import { ApplicationInsights } from '@microsoft/applicationinsights-web';

/**
 * @remarks
 * See:
 * https://www.npmjs.com/package/\@microsoft/applicationinsights-react-js
 * https://docs.microsoft.com/es-es/azure/azure-monitor/app/javascript-react-plugin
 * https://github.com/Azure-Samples/application-insights-react-demo/tree/master/src
 * https://github.com/microsoft/applicationinsights-js
 */
const reactPlugin = new ReactPlugin();
const appInsights = new ApplicationInsights({
  config: {
    instrumentationKey: process.env.REACT_APP_TELEMETRY_INSTRUMENTATION_KEY,
    maxBatchInterval: 0,
    disableFetchTracking: false,
    extensions: [reactPlugin],
  },
});

appInsights.loadAppInsights();

export { appInsights };