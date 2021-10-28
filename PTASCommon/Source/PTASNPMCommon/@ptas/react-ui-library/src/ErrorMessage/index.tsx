import { SnackbarProps, Snackbar as MUISnackBar } from "@material-ui/core";
import { omit } from "lodash";
import React, {
  Fragment,
  PropsWithChildren,
  SyntheticEvent,
  useEffect,
  useRef,
  useState
} from "react";
import { createContext } from "react";
import { makeStyles, Theme, Collapse } from "@material-ui/core";
import IconButton from "@material-ui/core/IconButton";
import CloseIcon from "@material-ui/icons/Close";
import clsx from "clsx";
import CustomButton from "./CustomButton";
import CustomTextButton from "../CustomTextButton";
import * as fm from "./formatText";

export type ErrorMessageState = {
  open: boolean;
  showButton?: boolean;
  onClickReport?: (error: string) => void;
  errorDesc?: string;
  portal?: PortalType;
};

type PortalType = "CS" | "GIS";

type ContextProps = {
  errorMessageState: ErrorMessageState;
  setErrorMessageState: React.Dispatch<React.SetStateAction<ErrorMessageState>>;
  showErrorMessage: (
    state:
      | Pick<ErrorMessageState, "onClickReport" | "errorDesc">
      | string
      | undefined,
    portal?: PortalType,
    showButton?: boolean
  ) => void;
};

export const ErrorMessageContext = createContext<ContextProps>(null as any);

/**
 * style component
 */
const useStyles = makeStyles((theme: Theme) => ({
  root: {
    maxWidth: "654px !important",
    width: "100%",
    fontFamily: theme.ptas.typography.bodyFontFamily
  },
  errorInfoRoot: {
    maxWidth: "700px !important"
  },
  content: {
    width: "100%",
    maxWidth: 461,
    margin: "0 auto"
  },
  errorInfoContent: {
    maxWidth: 532
  },
  errorInfo: {
    display: "flex",
    justifyContent: "space-between",
    alignItems: "center"
  },
  centeredItems: {
    justifyContent: "center"
  },
  wrapContent: {
    backgroundColor: "rgba(145, 12, 8, 0.8)",
    boxShadow: "0px 4px 16px rgba(0, 0, 0, 0.15)",
    backdropFilter: "blur(16px)",
    borderRadius: 9,
    width: "100%",
    position: "relative",
    paddingTop: 7,
    paddingBottom: 6
  },
  close: {
    position: "absolute",
    right: 7,
    top: 0,
    padding: 0
  },
  message: {
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    color: theme.ptas.colors.theme.white,
    maxWidth: 250
  },
  closeIcon: {
    fontSize: theme.ptas.typography.h4.fontSize,
    color: theme.ptas.colors.theme.white
  },
  reportButton: {
    width: 156,
    height: 23,
    fontSize: 14
  },
  errorInfoButton: {
    color: theme.ptas.colors.theme.white,
    fontSize: 14
  },
  expandMore: {
    color: theme.ptas.colors.theme.white,
    transition: "all 0.3s ease"
  },
  rotateIcon: {
    transform: "rotate(180deg)"
  },
  border: {
    background:
      "linear-gradient(90deg, rgba(255, 255, 255, 0) 0%, #FFFFFF 49.48%, rgba(255, 255, 255, 0) 100%)",
    width: 320,
    margin: "0 auto",
    height: 1,
    display: "block"
  },
  errorDesc: {
    padding: "19px 19px 27px 18px",
    color: theme.ptas.colors.theme.white,
    fontSize: 14
  }
}));

const SnackBar = (
  props: SnackbarProps & Omit<ErrorMessageState, "open">
): JSX.Element => {
  const classes = useStyles();
  /**
   * open-close error message
   */
  const [open, setOpen] = useState<boolean>(false);
  /**
   * show-hide error details
   */
  const [openErrorInfo, setOpenErrorInfo] = useState<boolean>(false);

  useEffect(() => {
    setOpen(!!props.open);

    if (!props.open) setOpenErrorInfo(false);
  }, [props.open]);

  /**
   * close error message
   * @param event
   * @returns void
   */
  const handleClose = (event: React.SyntheticEvent | MouseEvent) => {
    setOpen(false);

    props.onClose?.(event as SyntheticEvent<Element, Event>, "clickaway");
  };

  const handelOpenErrorInfo = () => setOpenErrorInfo((prev) => !prev);

  const handleReportError = () =>
    props.errorDesc && props.onClickReport?.(props.errorDesc);

  const renderProblemMessage = () => {
    if (props.portal === "CS") {
      return fm.problemHasOcurredCS;
    }
    return fm.problemHasOcurred;
  };

  const renderReportText = () => {
    if (props.portal === "CS") return fm.reportCS;
    return fm.report;
  };

  const renderContent = (): JSX.Element => {
    return (
      <div className={classes.wrapContent}>
        <div
          className={clsx(
            classes.content,
            props.errorDesc && classes.errorInfoContent
          )}
        >
          <div
            className={clsx(
              classes.errorInfo,
              !props.errorDesc && classes.centeredItems
            )}
          >
            <span className={classes.message}>{renderProblemMessage()}</span>
            {props.errorDesc && (
              <Fragment>
                {props.showButton && (
                  <CustomButton
                    ptasVariant='Danger inverse'
                    classes={{
                      root: classes.reportButton
                    }}
                    onClick={handleReportError}
                  >
                    {renderReportText()}
                  </CustomButton>
                )}
                <CustomTextButton
                  ptasVariant='Text more small'
                  onClick={handelOpenErrorInfo}
                  classes={{
                    root: classes.errorInfoButton,
                    expandMoreIcon: clsx(
                      classes.expandMore,
                      openErrorInfo && classes.rotateIcon
                    )
                  }}
                >
                  {fm.errorInfo}
                </CustomTextButton>
              </Fragment>
            )}
          </div>
        </div>
        <Collapse in={openErrorInfo}>
          <div className={classes.errorDesc}>
            <span className={classes.border}></span>
            {!!props.errorDesc && props.errorDesc}
          </div>
        </Collapse>
        <IconButton
          aria-label='close'
          color='inherit'
          className={classes.close}
          onClick={handleClose}
        >
          <CloseIcon className={classes.closeIcon} />
        </IconButton>
      </div>
    );
  };

  return (
    <MUISnackBar
      className={clsx(classes.root, props.errorDesc && classes.errorInfoRoot)}
      anchorOrigin={{ vertical: "top", horizontal: "center" }}
      autoHideDuration={20000}
      open={open}
      onClose={handleClose}
      {...omit(props, [
        "className",
        "open",
        "onClose",
        "errorDesc",
        "onClickReport"
      ])}
    >
      {renderContent()}
    </MUISnackBar>
  );
};

/** Provider */
export const ErrorMessageProvider = (
  props: PropsWithChildren<{
    onClickReport?: (error: string) => void;
    autoHideDuration?: number;
  }>
): JSX.Element => {
  const [errorMessageState, setErrorMessageState] = useState<ErrorMessageState>(
    {
      open: false,
      portal: "GIS",
      showButton: true
    }
  );

  const showErrorMessage = useRef(
    (
      state: Omit<ErrorMessageState, "open"> | string | undefined,
      portal: PortalType = "GIS",
      showButton: boolean = true
    ) => {
      if (typeof state === "string") {
        return setErrorMessageState({
          open: true,
          errorDesc: state,
          portal,
          showButton
        });
      }
      if (typeof state === "undefined") {
        return setErrorMessageState({
          open: true,
          portal,
          showButton
        });
      }
      return setErrorMessageState({ ...state, portal, showButton, open: true });
    }
  );

  const onClickReport = () => {
    if (typeof errorMessageState.errorDesc === "string") {
      if (errorMessageState.onClickReport) {
        return errorMessageState.onClickReport(errorMessageState.errorDesc);
      }
      return props.onClickReport?.(errorMessageState.errorDesc);
    }
  };

  return (
    <ErrorMessageContext.Provider
      value={{
        errorMessageState,
        setErrorMessageState,
        showErrorMessage: showErrorMessage.current
      }}
    >
      {props.children}
      <SnackBar
        open={errorMessageState.open}
        showButton={errorMessageState.showButton}
        portal={errorMessageState.portal}
        onClose={(): void => {
          setErrorMessageState((prev) => ({ ...prev, open: false }));
        }}
        onClickReport={onClickReport}
        errorDesc={errorMessageState.errorDesc}
        autoHideDuration={props.autoHideDuration}
      />
    </ErrorMessageContext.Provider>
  );
};
