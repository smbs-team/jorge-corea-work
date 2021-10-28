// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { FC } from "react";
import { withStyles, createStyles, Theme, WithStyles } from "@material-ui/core";
import Chip, { ChipProps } from "@material-ui/core/Chip";
import CloseIcon from "@material-ui/icons/Close";
import { GenericWithStyles } from "..";
import { omit } from "lodash";

interface Props extends WithStyles<typeof useStyles> {
  isSelected?: boolean;
}

const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: "1rem"
    },
    outlined: ({ isSelected }: Props) => ({
      borderColor: isSelected
        ? theme.ptas.colors.utility.selection
        : theme.ptas.colors.theme.black,
      backgroundColor: isSelected
        ? theme.ptas.colors.utility.selectionLight + " !important"
        : "transparent !important"
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

  return <Chip variant='outlined' deleteIcon={<CloseIcon />} {...newProps} />;
}

export default withStyles(useStyles)(CustomChip) as FC<CustomChipProps>;
