// StyledIconButton.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { PropsWithChildren } from 'react';
import { createStyles, makeStyles } from '@material-ui/core';
import { IconButton, IconButtonProps } from '@material-ui/core';
import { omit } from 'lodash';

/**
 * Component props
 */
interface Props {
  isSelected?: boolean;
}

/**
 * Component styles
 */
const useStyles = makeStyles(() =>
  createStyles({
    root: {
      width: 38,
      height: 38,
      backgroundColor: 'rgba(0,0,0,0.7)',
      color: (props: Props): string => {
        return props.isSelected
          ? 'rgba(165, 199, 39, 1)'
          : 'rgba(255,255,255, 1)';
      },
      '&:hover': {
        backgroundColor: 'rgba(0,0,0,0.7)',
        color: '#a5c727',
      },
    },
    label: {
      width: 32,
    },
  })
);

/**
 * StyledIconButton
 *
 * @param props - Component props
 * @returns A JSX element
 */
function StyledIconButton(
  props: PropsWithChildren<Props> & IconButtonProps
): JSX.Element {
  const { root, label } = useStyles(props);

  const newProps = omit(props, 'isSelected');

  return (
    <IconButton {...newProps} classes={{ root: root, label: label }}>
      {props.children}
    </IconButton>
  );
}

export default StyledIconButton;
