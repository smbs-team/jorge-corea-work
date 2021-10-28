/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import {
  Box,
  Popover,
  PopoverProps,
  Theme,
  StyleRules,
  PopoverClassKey,
  createStyles,
  withStyles,
  WithStyles
} from "@material-ui/core";
import React, { PropsWithChildren, useEffect, useState } from "react";
import { Close } from "@material-ui/icons";
import { omit } from "lodash";

const customClasses = ["closeButton", "closeIcon"] as const;

type ClassKey = PopoverClassKey | typeof customClasses[number];

const styles = (theme: Theme): StyleRules<ClassKey, Props> =>
  createStyles<ClassKey, Props>({
    paper: {},
    root: {},
    closeButton: {
      position: "absolute",
      top: theme.spacing(1),
      right: theme.spacing(1),
      cursor: "pointer",
      color: theme.ptas.colors.theme.black
    },
    closeIcon: {
      width: 30,
      height: 30
    }
  });

type Props = PropsWithChildren<PopoverProps> & {
  removeCloseIcon?: boolean;
};

function PtasPopover(props: Props & WithStyles<typeof styles>): JSX.Element {
  const { classes } = props;
  const { removeCloseIcon, ...muiPopoverProps } = props;
  const [isOpen, setIsOpen] = useState(props.open);

  useEffect(() => {
    setIsOpen(props.open);
  }, [props.open]);

  return (
    <Popover
      {...omit(muiPopoverProps, "classes")}
      classes={omit(classes, customClasses)}
      open={isOpen}
    >
      {props.children}
      {removeCloseIcon !== false && (
        <Box
          onClick={() => {
            setIsOpen(false);
          }}
          className={classes.closeButton}
        >
          <Close classes={{ root: classes.closeIcon }} />
        </Box>
      )}
    </Popover>
  );
}

export default withStyles(styles)(PtasPopover);
