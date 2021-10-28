/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { PropsWithChildren, useContext, useEffect } from 'react';
import { ErrorMessageContext } from '@ptas/react-public-ui-library';

const AppShowErrorMessage = (
  props: PropsWithChildren<{ showErrorMessage: boolean; message: string }>
): JSX.Element => {
  const { children, showErrorMessage, message } = props;
  const { setErrorMessageState } = useContext(ErrorMessageContext);

  useEffect(() => {
    if (!showErrorMessage) return;

    setErrorMessageState({
      open: showErrorMessage,
      errorHead: message,
    });

    // eslint-disable-next-line
  }, [showErrorMessage]);
  return children as JSX.Element;
};

export default AppShowErrorMessage;
