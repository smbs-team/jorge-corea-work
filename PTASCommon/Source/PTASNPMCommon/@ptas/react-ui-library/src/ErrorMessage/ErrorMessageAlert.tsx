import React, { Fragment, useState } from "react";
import {
  ButtonBase,
  makeStyles,
  Theme,
  IconButton,
  Collapse,
  Box
} from "@material-ui/core";
import CloseIcon from "@material-ui/icons/Close";
import CustomButton from "./CustomButton";
import ExpandMoreIcon from "@material-ui/icons/ExpandMore";

export interface ErrorMessageAlertProps {
  errorMessage: string;
  buttonText?: string;
  errorDetails?: string;
  moreInfoText?: string;
  onClose?: (event?: React.MouseEvent<HTMLButtonElement>) => void;
  onButtonClick?: (event?: React.MouseEvent<HTMLButtonElement>) => void;
}

const useStyles = makeStyles((theme: Theme) => ({
  root: {
    display: "flex",
    flexDirection: "column",
    width: 650,
    color: "white",
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    borderRadius: 9,
    backgroundColor: "rgba(145, 12, 8, 0.8)",
    boxShadow: "0px 4px 16px rgba(0, 0, 0, 0.15)",
    backdropFilter: "blur(16px)",
    padding: theme.spacing(1, 2.25, 1, 2.25)
  },
  content: {
    display: "flex",
    alignItems: "center"
  },
  reportButton: {
    margin: "0px 16px 0px 16px",
    height: 23,
    fontSize: 14,
    "&:hover": {
      boxShadow: "unset !important"
    }
  },
  closeButton: {
    padding: 0,
    color: "white",
    marginLeft: "auto"
  },
  errorMessage: {
    marginLeft: "auto"
  },
  moreInfoButton: {
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: theme.ptas.typography.bodySmall.fontSize
  },
  border: {
    background:
      "linear-gradient(90deg, rgba(255, 255, 255, 0) 0%, #FFFFFF 49.48%, rgba(255, 255, 255, 0) 100%)",
    width: 320,
    margin: "16px auto",
    height: 1
  },
  errorDetails: {
    fontSize: theme.ptas.typography.finePrint.fontSize,
    lineHeight: "15px"
  }
}));

function ErrorMessageAlert(props: ErrorMessageAlertProps) {
  const classes = useStyles();
  const [isDetailsOpen, setIsDetailsOpen] = useState<boolean>(false);

  return (
    <div className={classes.root}>
      <Box className={classes.content}>
        <div className={classes.errorMessage}>{props.errorMessage}</div>
        {props.onButtonClick !== undefined && (
          <CustomButton
            ptasVariant='Danger inverse'
            classes={{
              root: classes.reportButton
            }}
            onClick={props.onButtonClick}
          >
            {props.buttonText ?? "Refresh"}
          </CustomButton>
        )}
        {props.errorDetails && (
          <Fragment>
            <ButtonBase
              className={classes.moreInfoButton}
              onClick={() => setIsDetailsOpen(!isDetailsOpen)}
            >
              {props.moreInfoText ?? "Error Info"}
              <ExpandMoreIcon
                style={{
                  transform: isDetailsOpen ? "rotate(180deg)" : undefined
                }}
              />
            </ButtonBase>
          </Fragment>
        )}
        <IconButton className={classes.closeButton} onClick={props.onClose}>
          <CloseIcon />
        </IconButton>
      </Box>
      <Collapse in={isDetailsOpen}>
        <div className={classes.errorDetails}>
          <div className={classes.border} />
          {props.errorDetails}
        </div>
      </Collapse>
    </div>
  );
}

export default ErrorMessageAlert;
