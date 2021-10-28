/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, {
  FC,
  forwardRef,
  MutableRefObject,
  PropsWithChildren,
} from 'react';
import {
  createStyles,
  StyleRules,
  withStyles,
  WithStyles,
} from '@material-ui/core';
import { GenericWithStyles } from '@ptas/react-ui-library';

const styles = (): StyleRules<'root'> =>
  createStyles({
    root: {
      position: 'absolute',
      bottom: 0,
      width: '100%',
      height: '100%',
      overflow: 'hidden',
    },
  });

type Props = PropsWithChildren<{
  ref:
    | ((instance: HTMLDivElement | null) => void)
    | MutableRefObject<HTMLDivElement | null>
    | null;
}>;

const MapElement = forwardRef<
  HTMLDivElement,
  Props & WithStyles<typeof styles>
>(
  (props, ref): JSX.Element => {
    return <div id="map" ref={ref} className={props.classes.root} />;
  }
);

export default withStyles(styles)(MapElement) as FC<
  GenericWithStyles<Props & WithStyles<typeof styles>>
>;
