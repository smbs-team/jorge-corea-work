/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, {
  createContext,
  useState,
  PropsWithChildren,
  useEffect,
  useRef
} from "react";

const defaultErrorMessage = "Sorry, a problem has ocurred";

type Props = {
  defaultErrorMessage: string;
  errorMessage: string | undefined;
  setErrorMessage: React.Dispatch<React.SetStateAction<string | undefined>>;
  errorDetail: string | undefined;
  setErrorDetail: React.Dispatch<React.SetStateAction<string | undefined>>;
  showErrorMessage: (
    val:
      | string
      | undefined
      | {
          message?: string | undefined;
          detail?: string | undefined;
        }
  ) => void;
  open: boolean;
  setOpen: React.Dispatch<React.SetStateAction<boolean>>;
};

// eslint-disable-next-line @typescript-eslint/no-explicit-any
const ErrorMessageAlertCtx = createContext<Props>(null as any);
export default ErrorMessageAlertCtx;

export const ErrorMessageAlertProvider = (
  props: PropsWithChildren<unknown>
): JSX.Element => {
  const [errorMessage, setErrorMessage] = useState<string>();
  const [errorDetail, setErrorDetail] = useState<string>();
  const [open, setOpen] = useState<boolean>(false);

  const showErrorMessage = useRef(
    (val: string | undefined | { message?: string; detail?: string }): void => {
      if (typeof val === "string") {
        setErrorMessage(val);
        setErrorDetail(val);
      } else {
        setErrorDetail(val?.detail);
        setErrorMessage(val?.message ?? defaultErrorMessage);
      }
      setOpen(true);
    }
  );

  useEffect(() => {
    if (!open) {
      setErrorMessage(undefined);
      setErrorDetail(undefined);
    }
  }, [open]);

  return (
    <ErrorMessageAlertCtx.Provider
      value={{
        open,
        setOpen,
        showErrorMessage: showErrorMessage.current,
        errorDetail,
        setErrorDetail,
        errorMessage,
        setErrorMessage,
        defaultErrorMessage
      }}
    >
      {props.children}
    </ErrorMessageAlertCtx.Provider>
  );
};
