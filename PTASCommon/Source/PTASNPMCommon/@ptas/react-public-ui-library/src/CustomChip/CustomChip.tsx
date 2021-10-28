// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { FC, Fragment } from "react";
import { withStyles, createStyles, Theme, WithStyles } from "@material-ui/core";
import Chip, { ChipProps } from "@material-ui/core/Chip";
import CloseIcon from "@material-ui/icons/Close";
import { GenericWithStyles } from "@ptas/react-ui-library";
import { omit } from "lodash";

interface Props extends WithStyles<typeof useStyles> {
  isSelected?: boolean;
  useIcon?: boolean;
  ptasVariant?: "ligth" | "inverse";
}

const useStyles = (theme: Theme) =>
  createStyles({
    root: (props: Props) => ({
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: "1rem",
      color:
        props.ptasVariant === "inverse"
          ? theme.ptas.colors.theme.white
          : theme.ptas.colors.theme.black
    }),
    outlined: ({ isSelected, ptasVariant }: Props) => ({
      borderColor: isSelected
        ? theme.ptas.colors.utility.selection
        : theme.ptas.colors.theme.black,
      backgroundColor: isSelected
        ? `${theme.ptas.colors.utility.selection} !important`
        : ptasVariant === "inverse"
        ? theme.ptas.colors.theme.black
        : "transpatent !important"
    }),
    deleteIcon: {
      width: 16,
      height: 16,
      color: theme.ptas.colors.theme.black,
      "&:hover": {
        color: theme.ptas.colors.utility.selection
      }
    }
    // clickable: {
    //   "&:focus": {
    //     backgroundColor: (props: Props) =>
    //       props.isSelected
    //         ? theme.ptas.colors.utility.selectionLight + "!important"
    //         : "unset !important"
    //   }
    // }
  });

export type CustomChipProps = GenericWithStyles<Props & ChipProps>;

/**
 * CustomChip
 *
 * @param props - Component props
 * @returns A JSX element
 */

function CustomChip(props: Props & ChipProps): JSX.Element {
  const newProps = omit(props, "isSelected");
  const { classes } = props;

  return (
    <Chip
      variant='outlined'
      className={classes.root}
      deleteIcon={props.useIcon ? <CloseIcon /> : <Fragment />}
      {...newProps}
    />
  );
}

export default withStyles(useStyles)(CustomChip) as FC<CustomChipProps>;
