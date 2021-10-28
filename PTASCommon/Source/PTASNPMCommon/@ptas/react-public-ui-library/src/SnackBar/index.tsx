import { SnackbarProps, Snackbar as MUISnackBar } from "@material-ui/core";
import MuiAlert, { AlertProps } from "@material-ui/lab/Alert";
import { omit } from "lodash";
import React, { PropsWithChildren, useEffect, useState } from "react";
import { createContext } from "react";

export type SnackState =
  | {
      text: string;
      severity: AlertProps["severity"];
    }
  | undefined;

type ContextProps = {
  snackState: SnackState;
  setSnackState: React.Dispatch<React.SetStateAction<SnackState>>;
};

export const SnackContext = createContext<ContextProps>({
  setSnackState: (): void => {
    throw new Error("Unimplemented yet");
  },
  snackState: undefined
});

function Alert(props: AlertProps): JSX.Element {
  return <MuiAlert elevation={6} variant='filled' {...props} />;
}

const SnackBar = (
  props: SnackbarProps & Pick<AlertProps, "severity">
): JSX.Element => {
  const [severity, setSeverity] = useState<AlertProps["severity"]>();

  useEffect(() => {
    if (!props?.severity) return;
    setSeverity(props.severity);
  }, [props]);

  return (
    <MUISnackBar
      anchorOrigin={{ vertical: "top", horizontal: "right" }}
      autoHideDuration={30000}
      {...omit(props, "alertProps")}
    >
      {severity && (
        <Alert
          severity={severity}
          onClose={(event): void => {
            props.onClose && props.onClose(event, "clickaway");
          }}
        >
          {props.message}
        </Alert>
      )}
    </MUISnackBar>
  );
};

export const SnackProvider = (props: PropsWithChildren<{}>): JSX.Element => {
  const [snackState, setSnackState] = useState<SnackState>();
  return (
    <SnackContext.Provider
      value={{
        snackState,
        setSnackState
      }}
    >
      {props.children}
      <SnackBar
        message={snackState?.text}
        open={!!snackState?.text}
        onClose={(): void => {
          setSnackState(undefined);
        }}
        severity={snackState?.severity}
      />
    </SnackContext.Provider>
  );
};
