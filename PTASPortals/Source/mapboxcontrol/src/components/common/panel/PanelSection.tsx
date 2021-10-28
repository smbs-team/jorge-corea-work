// PanelSection.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { PropsWithChildren, forwardRef } from 'react';
import {
  withStyles,
  WithStyles,
  Theme,
  createStyles,
  StyleRules,
} from '@material-ui/core';

type Props = object & WithStyles<typeof useStyles>;

export const useStyles = (theme: Theme): StyleRules =>
  createStyles({
    root: {
      marginLeft: theme.spacing(4),
      marginTop: theme.spacing(20 / 8),
    },
  });

const PanelSection = forwardRef<HTMLDivElement, PropsWithChildren<Props>>(
  (props: PropsWithChildren<Props>, ref): JSX.Element => {
    return (
      <div ref={ref} className={props.classes.root}>
        {props.children}
      </div>
    );
  }
);

export default withStyles(useStyles)(PanelSection);
