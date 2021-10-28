/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { ApplicationInsights } from '@microsoft/applicationinsights-web';
import { ReactPlugin } from '@microsoft/applicationinsights-react-js';
import { createBrowserHistory } from 'history';

const browserHistory = createBrowserHistory({ basename: '' });
const reactPlugin = new ReactPlugin();

const appInsights = new ApplicationInsights({
  config: {
    instrumentationKey: process.env.REACT_APP_TELEMETRY_INSTRUMENTATION_KEY,
    enableAutoRouteTracking: true,
    maxBatchInterval: 0,
    disableFetchTracking: false,
    extensions: [],
    extensionConfig: {
      [reactPlugin.identifier]: { history: browserHistory },
    },
  },
});

appInsights.loadAppInsights();

export { reactPlugin, appInsights };
