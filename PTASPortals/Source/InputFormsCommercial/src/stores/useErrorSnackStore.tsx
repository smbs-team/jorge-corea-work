// useErrorSnackStore.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { SnackbarCloseReason } from '@material-ui/core';
import { SeverityLevel } from '@microsoft/applicationinsights-web';
import { appInsights } from 'api/AppInsights';
import { createTrackedSelector } from 'react-tracked';
import create from 'zustand';

export interface ErrorSnackStoreProps {
  isOpen: boolean;
  handleOnClose: (
    event?: React.SyntheticEvent<unknown, Event>,
    reason?: SnackbarCloseReason
  ) => void;
  errorMessage: string;
  errorDetails?: string;
  newError: (errorMessage?: string, errorDetails?: string) => void;
  handleOnClick: () => void;
}

const useStore = create<ErrorSnackStoreProps>((set, get) => ({
  isOpen: false,
  errorMessage: '',
  errorDetails: undefined,
  handleOnClose: (_event, reason) => {
    const errorDetails = get().errorDetails;
    if (errorDetails && errorDetails.length > 1 && reason === 'clickaway')
      return;
    set({ isOpen: false, errorDetails: undefined });
  },
  newError: (errorMessage = 'Sorry, a problem has occurred.', errorDetails) => {
    appInsights.trackException({
      exception: new Error(errorDetails),
      severityLevel: SeverityLevel.Error,
    });
    set({ isOpen: true, errorMessage, errorDetails });
  },
  handleOnClick: () => {
    set({ isOpen: false });
    window.location.reload();
  },
}));

const useErrorSnackStore = createTrackedSelector(useStore);

export default useErrorSnackStore;
