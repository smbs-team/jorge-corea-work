import React, { FC, Fragment } from "react";
import { Theme } from "@material-ui/core/styles";
import { Box, createStyles, WithStyles, withStyles } from "@material-ui/core";
import CustomCard from "./CustomCard";
import { GenericWithStyles } from "@ptas/react-ui-library";
import { CustomButton } from "../CustomButton";
import { CustomTextButton } from "../CustomTextButton";
import WarningIcon from "@material-ui/icons/Warning";
import clsx from "clsx";

export type CustomCardAction = {
  text: string;
  type: "button" | "label";
  buttonStyle?: "default" | "text";
  labelStyle?: "success" | "warning" | "error";
};

interface Props extends WithStyles<typeof styles> {
  title: string;
  text: string;
  actions: CustomCardAction[];
  /**
   * Event fired when clicking on an action
   */
  onActionClick?: (action: CustomCardAction) => void;
  shadow?: boolean;
}

/**
 * Component styles
 */
const styles = (theme: Theme) =>
  createStyles({
    // root: {
    //   width: 179,
    //   padding: theme.spacing(1, 2)
    // },
    rootWrap: {
      width: 179,
      height: "fit-content",
      padding: theme.spacing(1, 2),
      background: "rgba(255, 255, 255, 0.5)",
      borderRadius: "12px"
    },
    cardContent: {
      padding: theme.spacing(0) + " !important"
    },
    title: {
      marginBottom: theme.spacing(1),
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.bodySmall.fontSize,
      fontWeight: theme.ptas.typography.bodySmallBold.fontWeight,
      lineHeight: "19px",
      textAlign: "center"
    },
    text: {
      marginBottom: theme.spacing(1),
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.finePrint.fontSize,
      fontWeight: theme.ptas.typography.finePrintBold.fontWeight,
      lineHeight: "15px",
      textAlign: "center"
    },
    actions: {
      display: "flex",
      flexDirection: "column",
      justifyContent: "center",
      alignItems: "center"
    },
    action: {
      marginBottom: theme.spacing(1),
      "&:last-child": {
        marginBottom: 0
      }
    },
    defaultButton: {
      height: 23,
      padding: theme.spacing(3 / 8, 22 / 8),
      fontSize: 14
    },
    textButton: {
      fontSize: 14
    },
    texButtonSuccess: {
      color: theme.ptas.colors.utility.success
    },
    labelSuccess: {
      color: theme.ptas.colors.utility.success,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      // fontSize: theme.ptas.typography.body.fontSize,
      fontSize: 14,
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
      lineHeight: "17px"
    },
    labelError: {
      color: theme.ptas.colors.utility.danger,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      // fontSize: theme.ptas.typography.body.fontSize,
      fontSize: 14,
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
      lineHeight: "17px"
    },
    labelWarning: {
      color: theme.ptas.colors.theme.accent,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      // fontSize: theme.ptas.typography.body.fontSize,
      fontSize: 14,
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
      lineHeight: "17px"
    },
    warningIcon: {
      width: 14,
      height: 14,
      color: theme.ptas.colors.theme.accent
    }
  });

function CardWithButtons(props: Props): JSX.Element {
  const { classes } = props;

  return (
    <CustomCard
      variant={"wrapper"}
      shadow={props.shadow}
      classes={{
        rootWrap: classes.rootWrap,
        content: classes.cardContent
      }}
    >
      <Box className={classes.title}>{props.title}</Box>
      <Box className={classes.text}>{props.text}</Box>
      {props.actions && (
        <Box className={classes.actions}>
          {props.actions.map((action) => {
            return action.type === "button" ? (
              <Box
                className={classes.action}
                key={action.type + "-" + action.text}
              >
                {action.buttonStyle === "default" && (
                  <CustomButton
                    classes={{ root: classes.defaultButton }}
                    onClick={() => {
                      props.onActionClick && props.onActionClick(action);
                    }}
                  >
                    {action.text}
                  </CustomButton>
                )}
                {action.buttonStyle === "text" && (
                  <CustomTextButton
                    ptasVariant={"Text"}
                    onClick={() => {
                      props.onActionClick && props.onActionClick(action);
                    }}
                    classes={{
                      root: clsx(
                        classes.textButton,
                        action.labelStyle === "success" &&
                          classes.texButtonSuccess
                      )
                    }}
                  >
                    {action.text}
                  </CustomTextButton>
                )}
              </Box>
            ) : (
              <Box
                className={classes.action}
                key={action.type + "-" + action.text}
              >
                {action.labelStyle === "success" && (
                  <label className={classes.labelSuccess}>{action.text}</label>
                )}
                {action.labelStyle === "error" && (
                  <label className={classes.labelError}>{action.text}</label>
                )}
                {action.labelStyle === "warning" && (
                  <Fragment>
                    <WarningIcon className={classes.warningIcon} />{" "}
                    <label className={classes.labelWarning}>
                      {action.text}
                    </label>
                  </Fragment>
                )}
              </Box>
            );
          })}
        </Box>
      )}
    </CustomCard>
  );
}
export default withStyles(styles)(CardWithButtons) as FC<
  GenericWithStyles<Props>
>;
