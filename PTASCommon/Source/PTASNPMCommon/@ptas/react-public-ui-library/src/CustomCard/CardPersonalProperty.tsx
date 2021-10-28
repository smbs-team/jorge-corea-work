import React, { FC, ReactNode } from "react";
import { Theme } from "@material-ui/core/styles";
import { Box, createStyles, WithStyles, withStyles } from "@material-ui/core";
import CustomCard from "./CustomCard";
import { CustomButton } from "../CustomButton";
import { CustomTextButton } from "../CustomTextButton";
import { GenericWithStyles } from "@ptas/react-ui-library";

interface Props extends WithStyles<typeof styles> {
  type: "business" | "person";
  title: string;
  subtitle: string | ReactNode;
  shadow?: boolean;
  filedDate?: string;
  assessedYear?: string;
  disabledOutlineButton?: boolean;
  onOutlineButtonClick: (event: React.MouseEvent<HTMLButtonElement>) => void;
  onTextButtonClick: (event: React.MouseEvent<HTMLButtonElement>) => void;
  outlineButtonText: string | React.ReactNode;
  textButtonText: string | React.ReactNode;
}

/**
 * Component styles
 */
const styles = (theme: Theme) =>
  createStyles({
    root: (props: Props) => ({
      width: props.type === "business" ? "331px" : "243px",
      height: props.type === "business" ? "151px" : "111px"
    }),
    cardContent: (props: Props) => ({
      padding:
        props.type === "business"
          ? theme.spacing(2, 4) + " !important"
          : theme.spacing(2) + " !important"
    }),
    title: (props: Props) => ({
      marginBottom: theme.spacing(1),
      fontFamily: theme.ptas.typography.titleFontFamily,
      fontSize: props.type === "business" ? "24px" : "20px",
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
      lineHeight: props.type === "business" ? "28px" : "23px"
    }),
    subtitle: {
      marginBottom: theme.spacing(1),
      fontFamily: theme.ptas.typography.bodyFontFamily,
      lineHeight: "17px",
      fontSize: "14px",
      color: "rgba(0, 0, 0, 0.5)"
    },
    dates: {
      marginBottom: theme.spacing(1),
      fontSize: theme.ptas.typography.bodySmallBold.fontSize,
      fontWeight: theme.ptas.typography.bodySmallBold.fontWeight,
      lineHeight: "19px"
    },
    filedDate: {
      marginRight: theme.spacing(4)
    },
    buttons: {
      display: "flex",
      justifyContent: "space-between"
    },
    outlineButton: (props: Props) => ({
      minWidth: props.type === "business" ? "92px" : "68px",
      height: 23,
      padding: "3px 22px",
      fontSize: 14,
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
      lineHeight: "17px"
      // marginRight: theme.spacing(4)
    }),
    textButton: {
      color: theme.ptas.colors.theme.black,
      fontSize: theme.ptas.typography.bodySmallBold.fontSize,
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
      lineHeight: "19px"
    }
  });

function CardPersonalProperty(props: Props): JSX.Element {
  const { classes } = props;

  return (
    <CustomCard
      shadow={props.shadow}
      classes={{ root: classes.root, content: classes.cardContent }}
    >
      <Box className={classes.title}>{props.title}</Box>
      <Box className={classes.subtitle}>{props.subtitle}</Box>
      {props.type === "business" && (
        <Box className={classes.dates}>
          <label className={classes.filedDate}>
            {`Filed ${props.filedDate || "-"}`}
          </label>
          <label>Assessed {props.assessedYear}</label>
        </Box>
      )}
      <Box className={classes.buttons}>
        <CustomButton
          ptasVariant='Outline'
          classes={{ root: classes.outlineButton }}
          onClick={(event) => props.onOutlineButtonClick(event)}
          disabled={props.disabledOutlineButton}
        >
          {props.outlineButtonText}
        </CustomButton>
        <CustomTextButton
          ptasVariant='Text more'
          classes={{ root: classes.textButton }}
          onClick={(event) => props.onTextButtonClick(event)}
        >
          {props.textButtonText}
        </CustomTextButton>
      </Box>
    </CustomCard>
  );
}
export default withStyles(styles)(CardPersonalProperty) as FC<
  GenericWithStyles<Props>
>;
